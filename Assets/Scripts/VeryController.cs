using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;


[RequireComponent(typeof(Rigidbody))]
public class VeryController : MonoBehaviour
{

    public float RideHeight;
    public float RideSpringStrenght;
    public float RideSpringDamper;
    [Header("UpRight")] 
    public float Elapsed;

    public float UprightJointSpringStrength;
    public float UprightJointSpringDamper;
    public float UpForce = 10;
    private Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Ray ray = new Ray(transform.position, Vector3.up * -3);
        RaycastHit hit;
        Debug.DrawRay(transform.position ,Vector3.up * -3 ,Color.red  );

        if (Physics.Raycast(ray, out hit,2))
        {
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

        UpdateUprightForce();
    }

    private void UpdateUprightForce()
    {
        Quaternion characterCurrent = transform.rotation;
        Quaternion toGoal = ShortestRotation(Quaternion.identity, characterCurrent);

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
    }
}
