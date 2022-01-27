using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float AxisX, AxisZ;
    private Rigidbody rb;
    PlayerInput playerInput;
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
        _camera.GetComponent<CameraScript>().AddPlayerToList(gameObject);
        rb = gameObject.GetComponent<Rigidbody>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        Debug.Log(playerInput.currentControlScheme);
    }

    void Update()
    {
        rb.AddForce(AxisX * 600, 0, AxisZ * 600);
    }

    public void OnMovement(InputValue val)
    {
        if (playerInput.currentControlScheme == "XboxController")
        {
            AxisZ = val.Get<Vector2>().y * Time.deltaTime;
            AxisX = val.Get<Vector2>().x * Time.deltaTime;
        }
    }

    public void OnMovementKeyboard(InputValue val)
    {
        Debug.Log("Pouet");
        AxisZ = val.Get<Vector2>().y * Time.deltaTime ;
        AxisX = val.Get<Vector2>().x * Time.deltaTime;
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
