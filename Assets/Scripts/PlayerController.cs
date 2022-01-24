using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float AxisX, AxisZ;
    private Rigidbody rb;
    PlayerInput playerInput;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        Debug.Log(playerInput.currentControlScheme);
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(AxisX, 0, AxisZ);
    }

    public void OnMovement(InputValue val)
    {
        if (playerInput.currentControlScheme == "XboxController")
        {
            AxisZ = val.Get<Vector2>().y;
            AxisX = val.Get<Vector2>().x;
        }

        if (playerInput.currentControlScheme == "KeyboardMouse")
        {
            AxisZ = val.Get<float>() * 5;
        }
    }

    public void OnGrabObject(InputValue val)
    {
        Debug.Log("Je chope un objet");
        rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
    }

    public void OnHit(InputValue val)
    {
        Debug.Log("Je cogne dur");
    }
}
