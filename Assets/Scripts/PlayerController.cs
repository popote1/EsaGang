using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float AxisX, AxisZ;
    private Rigidbody rb;
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(AxisX, 0, AxisZ);
    }

    public void OnLeftStick(InputValue val)
    {
        AxisZ = val.Get<Vector2>().y;
        AxisX = val.Get<Vector2>().x;
    }
}
