using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerScript : MonoBehaviour
{
    private PlayerInputManager _inputManager;
    public GameObject camera;
    
    void Start()
    {
        _inputManager = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
