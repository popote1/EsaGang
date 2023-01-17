using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCandidatV2 : MonoBehaviour
{
    //GameObject rigidbody
    private Rigidbody _rb;
    
    //Armature rigidbody
    private Rigidbody _armatureRb;
    
    //Movement stuff
    [SerializeField] private float speedMultiplier;
    public float xSpeed;
    public float zSpeed;
    //[SerializeField] private IKFootSolver[] footSolvers;
    [SerializeField] private float movementMultiplier;
    
    [Header("UpRight")] 
    public float UprightJointSpringStrength;
    public float UprightJointSpringDamper;

    //Limbs stuff
    [Header("Limbs Stuff")] [SerializeField]
    private float limbsMaxVelocity = 50f;
    [SerializeField] List<GameObject> limbsGameObjectsList = new List<GameObject>();
    [SerializeField] List<Rigidbody> limbsRb = new List<Rigidbody>();

    public List<LegIk> Legs;
    //[SerializeField] List<LimbsScript> _limbsScriptsList = new List<LimbsScript>();
    
    //Jump stuff
    [Header("Jump Stuff")]
    [SerializeField] private float jumpSpeed;
    
    private Quaternion _forward;
    //public GroundCheckScript GroundCheckLeft, GroundCheckRight;
   public bool isGrounded
   {
       get
       {
           return transform.position.y < 1.1f;
           //if (GroundCheckLeft.isGrounded || GroundCheckRight.isGrounded)
           //{
           //    return true;
           //}


           //else
           //{
           //    return false;
           //}
       }
   }
    
    void Start()
    {
        foreach (Rigidbody rb in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            limbsGameObjectsList.Add(rb.gameObject);
            limbsRb.Add(rb);
            //if (rb.GetComponent<LimbsScript>())
            //{
            //    _limbsScriptsList.Add(rb.GetComponent<LimbsScript>());
            //}
        }

        _rb = GetComponent<Rigidbody>();
        _armatureRb = GetComponentInChildren<Rigidbody>();

    }

    private void Update()
    {
       Move();
       //ClampLimbsVelocity();
    }

    
    //METHODE DE MOUVEMENT
    void Move()
    {
        _rb.velocity = new Vector3(xSpeed * speedMultiplier, _rb.velocity.y, zSpeed * speedMultiplier);
        _forward = Quaternion.LookRotation(_rb.velocity);
    }

    void ClampLimbsVelocity()
    {
        float xLimbsVelocity = Mathf.Clamp(_rb.velocity.x, -limbsMaxVelocity, limbsMaxVelocity);
        float yLimbsVelocity = Mathf.Clamp(_rb.velocity.y, -limbsMaxVelocity, limbsMaxVelocity);
        float zLimbsVelocity = Mathf.Clamp(_rb.velocity.z, -limbsMaxVelocity, limbsMaxVelocity);

        foreach (Rigidbody rb in limbsRb)
        {
            rb.velocity = new Vector3(xLimbsVelocity, yLimbsVelocity, zLimbsVelocity);
        }
    }

    //private void FixedUpdate()
    //{
    //    UpdateUprightForce();
    //}
    //HORIZONTAL INPUT
    //void OnHorizontal(InputValue value)
    //{
    //    xSpeed = value.Get<float>();
    //}
    //
    ////VERTICAL INPUT
    //void OnVertical(InputValue value)
    //{
    //    zSpeed = value.Get<float>();
    //}

    private void OnMove(InputValue value) {
        xSpeed = value.Get<Vector2>().x;
        zSpeed = value.Get<Vector2>().y;
    }
    
    public void OnJump(InputValue val) {
        _rb.AddForce(Vector3.up * jumpSpeed * 10f, ForceMode.Impulse);

        foreach (var leg in Legs) {
            leg.SetJump();
        }
    }
    
    public void Bumping(Vector3 bumpingContact, GameObject bumpingSource)
    {
        foreach (Rigidbody rb in limbsRb)
        {
            GameObject attachedGO = rb.gameObject;
            if (attachedGO.GetComponent<ConfigurableJoint>())
            {
                ConfigurableJoint joint = attachedGO.GetComponent<ConfigurableJoint>();
                JointDrive jointDriveX = joint.angularXDrive;
                jointDriveX.positionSpring = 1f; 
                joint.angularXDrive = jointDriveX;
                
                JointDrive jointDriveYZ = joint.angularYZDrive;
                jointDriveYZ.positionSpring = 1f;
                joint.angularYZDrive = jointDriveYZ;
                
            }
        }
        
        bumpingSource.GetComponent<Rigidbody>().AddExplosionForce(5000, bumpingContact, 1f);
        StartCoroutine(GetUpCoolDown());
        
        Debug.Log("JE RAGDOLL");
    }

    private IEnumerator GetUpCoolDown()
    {
        yield return new WaitForSecondsRealtime(1f);
        GetUp();
    }
    
    public void GetUp()
    {
        foreach (Rigidbody rb in limbsRb)
        {
            GameObject attachedGO = rb.gameObject;
            if (attachedGO.GetComponent<ConfigurableJoint>())
            {
                ConfigurableJoint joint = attachedGO.GetComponent<ConfigurableJoint>();
                JointDrive jointDriveX = joint.angularXDrive;
                jointDriveX.positionSpring = 550f;
                joint.angularXDrive = jointDriveX;
                
                JointDrive jointDriveYZ = joint.angularYZDrive;
                jointDriveYZ.positionSpring = 550f;
                joint.angularYZDrive = jointDriveYZ;
            }
        }
    }
    private void UpdateUprightForce()
    {
        Quaternion characterCurrent = transform.rotation;
        Quaternion toGoal = ShortestRotation(_forward, characterCurrent);

        Vector3 rotAxis;
        float rotDegrees;

        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();

        float rotRadians = rotDegrees * Mathf.Rad2Deg;

        _rb.AddTorque((rotAxis * (rotRadians * UprightJointSpringStrength)) -
                      (_rb.angularVelocity * UprightJointSpringDamper));

    }
    
    public Quaternion ShortestRotation(Quaternion a, Quaternion b)
    {
        if (Quaternion.Dot(a, b) < 0)
        {
            return a * Quaternion.Inverse(Multiply(b, -1));
        }
        else return a * Quaternion.Inverse(b);
    }
    public Quaternion Multiply(Quaternion input, float scalar)
    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }

}
