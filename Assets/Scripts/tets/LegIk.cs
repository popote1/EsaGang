using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegIk : MonoBehaviour
{
    public PlayerCandidatV2 PlayerControler;
    public Transform Raycater;
    public Transform JumpFeetPos;
    public LayerMask GroundMask;
    public GameObject Feet;
    public Rigidbody RB;
    public float RaycastDiformationmultiplier = 0.25f;

    public float FeetDistance = 1;
    public bool Move;
    public bool IsJumping;
    public float FeetMoveTime = 0.5f;
    public LegIk LautreJambe;
    

    public AnimationCurve FeetRise;

    private float _timer;
    private Vector3 _feetbestPos;
    private Vector3 _feetPastPos;


    private void Update()
    {
        if (IsJumping) {
            Jump();
        }
        else
        {
            ManageFeetDistance();
            if (Move) MoveTheFeet();
        }
    }

    private void ManageFeetDistance()
    {
        Vector3 vectorOrientation = Raycater.position - new Vector3(0, -1, 0);
        vectorOrientation += (new Vector3(-RB.velocity.x, 0, -RB.velocity.z))*RaycastDiformationmultiplier;

        Vector3 rayDirection =  Raycater.position-vectorOrientation ;
        
        Ray ray = new Ray(Raycater.position, rayDirection);
        
        Debug.DrawRay(Raycater.position, rayDirection,Color.red);
        RaycastHit hit;
        if(Physics.Raycast(ray ,out hit ,10 , GroundMask ))
        {
            _feetbestPos = hit.point;
            Debug.DrawLine(Feet.transform.position, hit.point, Color.green);
            if (Vector3.Distance(Feet.transform.position, hit.point)>FeetDistance) {
                if (!Move &&!LautreJambe.Move) {
                    Move = true;
                    _feetPastPos = Feet.transform.position;
                    _timer = 0;
                }
            }
        }
    }

    private void MoveTheFeet()
    {
        _timer += Time.deltaTime;
        float time = _timer / FeetMoveTime;
        Vector3 feetpos = Vector3.Lerp( _feetPastPos,_feetbestPos,time);

        float y = FeetRise.Evaluate(time);
        feetpos.y += y;

        Feet.transform.position = feetpos;
        if(time>=1) {
            Move = false;
        }
    }

    private void Jump() {
        if (PlayerControler.isGrounded) {
            IsJumping = false;
        }
        Feet.transform.position = JumpFeetPos.transform.position;
        Feet.transform.localRotation = JumpFeetPos.transform.localRotation;
    }

    public void SetJump() {
        IsJumping = true;
    }
}
