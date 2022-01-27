using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class VeryController3 : MonoBehaviour
{

    public PlayerInputCommands PlayerInputCommands;
    
    public float RideHeight;
    public float RideSpringStrenght;
    public float RideSpringDamper;
    [Header("UpRight")]
    public float UprightJointSpringStrength;
    public float UprightJointSpringDamper;
    public bool IsGrounded;
    [Header("MovementParameters")] 
    public float MaxSpeed=8;
    public float Acceleration=200;
    [Header("Dasher Parameters")] 
    public float DashForce;
    public float DashOutOfCOntrolTime;
    [Header("ThrowObjcts")] 
    public Grabable Grabable;
    public Rigidbody Hand;
    public float Throwforce;
    [Header("Hp")] 
    public int MaxHP = 10;
    public int CurrentHP=10;
    public bool IsAlive = true;
    public float vibrationTime = 0.5f;


    public Text VelocityDisplay;
    [Header("Graphics")] 
    public SkinnedMeshRenderer TchirtMeshRenderer;
    public SkinnedMeshRenderer PlayerMeshRenderer;

    
    private Rigidbody _rb;
    private Quaternion _forward;
    Vector3 _move ;
    private Vector3 _handOriginalPos;
    private float _outOfControlTimer=0;
    private float _vibrationTimer=0;
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
        VelocityDisplay.text = CurrentHP + " / " + MaxHP;
        if (_vibrationTimer > 0)
        {
            _vibrationTimer -= Time.deltaTime;
            if (_vibrationTimer < 0)
            {
                _vibrationTimer = 0;
                Gamepad.all[PlayerInputCommands.PlayerInput.playerIndex].SetMotorSpeeds(0,0);
            }
        }
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

        if (IsAlive) {
            FlowtScript();
            Movement(_move);
            UpdateUprightForce();
        }
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
       actualVel.y = _rb.velocity.y;
       _rb.velocity = actualVel;
       
    }
    public void OnMovement(Vector2 val)
    {
        _move =Vector3.zero;
        _move.z = val.y;
        _move.x = val.x;
        
    }
    public void OnPunch()
    {
        Debug.Log("Punsh");
        if (_punchtimer != 0) return;
    }
    public void OnHit()
    {
        if (_outOfControlTimer == 0&& IsReady&&IsAlive) {
            _rb.AddForce(transform.forward * DashForce, ForceMode.Impulse);
            _outOfControlTimer += DashOutOfCOntrolTime;
        }
    }
    public void OnGrabObject()
    {
        Debug.Log("Grab Object");

        if ((_punchtimer != 0&& IsReady)||!IsAlive) return;
        if (Grabable == null) {
            Collider[] objectToGrabes = Physics.OverlapSphere(transform.forward + transform.position, 1);
            foreach (Collider col in objectToGrabes) {
                if (col.GetComponent<Grabable>() && col.transform != transform) {
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
            Grabable.Throwed = true;
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

    public void TakeDamage(int damage) {
        CurrentHP = CurrentHP - damage;
        if (CurrentHP <= 0)
        {
            IsAlive = false;
            CurrentHP = 0;
        }
        Gamepad.all[PlayerInputCommands.PlayerInput.playerIndex].SetMotorSpeeds(0.5f,0.5f);
        _vibrationTimer += vibrationTime;

        // InfoPanel.SetHP(CurrentHP);
    }
    
    
}
