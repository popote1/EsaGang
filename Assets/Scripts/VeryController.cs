using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class VeryController : MonoBehaviour
{

    public PlayerInput playerInput;
    public float RideHeight;
    public float RideSpringStrenght;
    public float RideSpringDamper;
    [Header("UpRight")]
    public float UprightJointSpringStrength;
    public float UprightJointSpringDamper;
    [Header("MovementParameters")] 
    public float MaxSpeed=8;
    public float Acceleration=200;
    public AnimationCurve AccelerationFactorFromDot;
    public float MaxAccelForce=150;
    public AnimationCurve MaxAccelerationForceFactorFromDot;
    public Vector3 ForceScale;
    public bool IsGrounded;
    public float JumpForce = 20;
    [Header("Dasher Parameters")] 
    public float DashForce;
    public float DashOutOfCOntrolTime;
    [Header("ThrowObjcts")] 
    public Grabable Grabable;
    public Rigidbody Hand;
    public float Throwforce;
    [Header("PunchParameters")] 
    public AnimationCurve PuchCurve;
    public float PunchTime=0.5f;
    

    public Text VelocityDisplay;
    
    private Rigidbody _rb;
    private Quaternion _forward;
    Vector3 _move ;
    private Vector3 _handOriginalPos;
    private float _outOfControlTimer=0;
    private float _punchtimer;
    private bool IsReady;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        IsReady = true;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void UpdateUprightForce()
    {
        Quaternion characterCurrent = transform.rotation;
        Quaternion toGoal = ShortestRotation(_forward,characterCurrent) ;

        Vector3 rotAxis;
        float rotDegrees;
        
        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();

        float rotRadians = rotDegrees * Mathf.Rad2Deg;
        
        _rb.AddTorque((rotAxis*(rotRadians*UprightJointSpringStrength))-(_rb.angularVelocity*UprightJointSpringDamper));

    }
    
    public Quaternion ShortestRotation(Quaternion a, Quaternion b) {
        if (Quaternion.Dot(a, b) < 0) {
            return a * Quaternion.Inverse(Multiply(b, -1));
        }
        else return a * Quaternion.Inverse(b);
    }
    public Quaternion Multiply(Quaternion input, float scalar) {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }

    private void FixedUpdate() {
        if (_outOfControlTimer > 0) {
            _outOfControlTimer -= Time.fixedDeltaTime;
            if (_outOfControlTimer < 0) _outOfControlTimer = 0;
            return;
        }
        FlowtScript();
        Movement(_move);
        UpdateUprightForce();
    }

    public void Movement( Vector3 move)
    {
        

       if (move.magnitude > 1) {
           move.Normalize();
       }

       

       _rb.AddForce(move*Acceleration);

       Vector3 actualVel = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
       if (move.magnitude > 0.5f)
       {
           _forward =Quaternion.LookRotation(move);
           actualVel = Vector3.ClampMagnitude(actualVel, MaxSpeed);
       }
       else
       {
           actualVel = Vector3.ClampMagnitude(actualVel, actualVel.magnitude/1.1f);
       }
       VelocityDisplay.text = actualVel.magnitude+"";
       actualVel.y = _rb.velocity.y;
       _rb.velocity = actualVel;
    }
    public void OnMovement(InputValue val)
    {
        _move =Vector3.zero;
        if (playerInput.currentControlScheme == "XboxController")
        {
            _move.z = val.Get<Vector2>().y;
            _move.x = val.Get<Vector2>().x;
        }
    }

    public void OnPunch(InputValue val)
    {
        Debug.Log("Punsh");
        if (_punchtimer != 0) return;
    }

    public void OnHit(InputValue val)
    {
        if (_outOfControlTimer == 0&& IsReady) {
            _rb.AddForce(transform.forward * DashForce, ForceMode.Impulse);
            _outOfControlTimer += DashOutOfCOntrolTime;
        }
    }
    public void OnGrabObject(InputValue val)
    {
        Debug.Log("Grab Object");

        if (_punchtimer != 0&& IsReady) return;
        if (Grabable == null)
        {
            Collider[] objectToGrabes = Physics.OverlapSphere(transform.forward + transform.position, 1);
            foreach (Collider col in objectToGrabes)
            {
                if (col.GetComponent<Grabable>() && col.transform != transform)
                {
                    Grabable = col.GetComponent<Grabable>();
                    SpringJoint joint =Grabable.GetSpringJoint();
                    Grabable.SpringJoint.connectedAnchor = Vector3.up*10;
                    Grabable.Rigidbody.mass = Grabable.Rigidbody.mass / 10;
                    Grabable.IsGrabed = true;
                    
                    joint.connectedBody = Hand;
                    joint.connectedAnchor =Vector3.zero;
                    return;
                }
            }
        }
        else {
            Destroy(Grabable.SpringJoint);
            Grabable.Rigidbody.mass = Grabable.Rigidbody.mass * 10;
            Grabable.Rigidbody.AddForce(transform.forward.normalized*(Throwforce* Grabable.Rigidbody.mass),ForceMode.Impulse);
            _rb.AddForce(-transform.forward*(Throwforce* Grabable.Rigidbody.mass)/10,ForceMode.Impulse);
            Grabable.IsGrabed = false;
            Grabable = null;
        }
    }

    

    public void FlowtScript()
    {
        Ray ray = new Ray(transform.position, Vector3.up * -3);
        RaycastHit hit;
        Debug.DrawRay(transform.position ,Vector3.up * -3 ,Color.red  );

        if (Physics.Raycast(ray, out hit,2))
        {
            IsGrounded = (hit.distance < RideHeight + 0.5); 
            Debug.DrawLine(transform.position ,hit.point,Color.blue);
            Vector3 vel = _rb.velocity;
            Vector3 rayDir = ray.direction;
            
            Vector3 othervel = Vector3.zero;
            Rigidbody hitbody = hit.rigidbody;
            if (hitbody != null) othervel = hitbody.velocity;

            float rayDirVel = Vector3.Dot(rayDir , vel);
            float otherDirVel = Vector3.Dot(rayDir, othervel);

            float relVel = rayDirVel - otherDirVel;

            float x = hit.distance - RideHeight;

            float springForce = (x * RideSpringStrenght) - (relVel * RideSpringDamper);
            
            _rb.AddForce(rayDir*springForce);
        }
    }
}
