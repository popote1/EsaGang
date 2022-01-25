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
    public SpringJoint Handjoint;
    public Rigidbody GrabObject;
    public float Throwforce;
    

    public Text VelocityDisplay;
    
    private Rigidbody _rb;
    private Quaternion _forward;
    Vector3 _move ;
    private float _outOfControlTimer=0;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
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

    private void FixedUpdate()
    {
        
        if (_outOfControlTimer > 0)
        {
            _outOfControlTimer -= Time.fixedDeltaTime;
            if (_outOfControlTimer < 0) _outOfControlTimer = 0;
            return;
        }

        FlowtScript();
        Movement(_move);
        UpdateUprightForce();
        
    }

    private void Movement( Vector3 move)
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
       /*
     

      float dotProduct = Vector3.Dot(actualVel, move);

      float accel = Acceleration * AccelerationFactorFromDot.Evaluate(dotProduct);
      Vector3 goalVel = Vector3.MoveTowards(actualVel, move, accel * Time.fixedDeltaTime);

      Vector3 needAccel = (goalVel -actualVel)/Time.fixedDeltaTime;
      
      //float maxAccel = MaxAccelForce *MaxAccelerationForceFactorFromDot.Evaluate(dotProduct)
      needAccel = Vector3.ClampMagnitude(needAccel,  MaxAccelForce);
      Debug.Log("Force add"+needAccel.magnitude);
      _rb.AddForce(needAccel);
      
      
      */



       /*
       Vector3 m_UniGoal = move;
       Vector3 m_GoalVel = _rb.velocity;

       Vector3 unitVel = m_GoalVel.normalized;

       float velDot = Vector3.Dot(m_UniGoal, unitVel);

       Vector3 goalVel = m_UniGoal * MaxSpeed * 1;

       m_GoalVel = Vector3.MoveTowards(m_GoalVel, (goalVel) + (Vector3.down), Acceleration);

       Vector3 needAccel = (m_GoalVel - _rb.velocity) / Time.deltaTime;

       float maxAccel = MaxAccelForce * MaxAccelerationForceFactorFromDot.Evaluate(velDot) * 10;

       needAccel = Vector3.ClampMagnitude(needAccel, MaxAccelForce);
       
       _rb.AddForce(Vector3.Scale(needAccel*_rb.mass,ForceScale ));
       */
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

    public void OnHit(InputValue val)
    {
        if (_outOfControlTimer == 0) {
            _rb.AddForce(transform.forward * DashForce, ForceMode.Impulse);
            _outOfControlTimer += DashOutOfCOntrolTime;
        }
    }
    public void OnGrabObject(InputValue val)
    {
        Debug.Log("Grab Object");

        if (GrabObject == null)
        {
            Collider[] objectToGrabes = Physics.OverlapSphere(transform.forward + transform.position, 1);
            foreach (Collider col in objectToGrabes)
            {
                if (col.GetComponent<Rigidbody>() && col.transform != transform)
                {
                    GrabObject = col.GetComponent<Rigidbody>();
                    Vector3 pos = GrabObject.position;
                    GrabObject.transform.position = Handjoint.transform.position;
                    GrabObject.mass =GrabObject.mass/ 10;
                    Handjoint.connectedBody = GrabObject;
                    GrabObject.transform.position = pos;
                    return;
                }
            }
        }
        else {
            Handjoint.connectedBody = null;
            GrabObject.mass =GrabObject.mass* 10;
            GrabObject.AddForce(transform.forward.normalized*(Throwforce*GrabObject.mass),ForceMode.Impulse);
            _rb.AddForce(-transform.forward*(Throwforce*GrabObject.mass)/10,ForceMode.Impulse);
            GrabObject = null;
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
