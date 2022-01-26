using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public List<GameObject> playersInGame = new List<GameObject>();
    public float baryX, baryZ;
    public float minZoom, maxZoom;
    public Vector3 barycentric;

    public CinemachineVirtualCamera virtualCam;
    private CinemachineComponentBase componentBase;
    
    [SerializeField] GameObject farthestPlayer1, farthestPlayer2;

    private float cameraDistance;
    //[SerializeField] private float sensitivity;

    public GameObject testBary;
    private void Awake()
    {
    }

    void Start()
    {
        componentBase = virtualCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        //virtualCam.transform.rotation = quaternion.
    }

    public void AddPlayerToList(GameObject player)
    {
        
        playersInGame.Add(player);
    }

    private void FixedUpdate()
    {
        if (playersInGame.Count != 0)
        {
            foreach (GameObject gameObject in playersInGame)
            {
                baryX += gameObject.transform.position.x;
                baryZ += gameObject.transform.position.z;

                if (farthestPlayer1 == null)
                {
                    farthestPlayer1 = gameObject;
                }

                if (farthestPlayer2 == null)
                {
                    farthestPlayer2 = gameObject;
                }
                if (gameObject.transform.position.x < farthestPlayer1.transform.position.x)
                {
                    farthestPlayer1 = gameObject;
                }

                if (gameObject.transform.position.x > farthestPlayer2.transform.position.x)
                {
                    farthestPlayer2 = gameObject;
                }
            }
            
            baryX = baryX / playersInGame.Count;
            baryZ = baryZ / playersInGame.Count;
            
            barycentric.x = baryX;
            barycentric.z = baryZ;

        }

        float baryHeight = Vector3.Magnitude(farthestPlayer1.transform.position - farthestPlayer2.transform.position);
        
        Debug.Log(baryHeight);
        baryHeight = Mathf.Clamp(baryHeight, 5f, 30f);

        testBary.transform.position = barycentric;

        //transform.position = testBary.transform.position;

        (componentBase as CinemachineFramingTransposer).m_CameraDistance = baryHeight;
        (componentBase as CinemachineFramingTransposer).m_TrackedObjectOffset.y = baryHeight;
        (componentBase as CinemachineFramingTransposer).m_TrackedObjectOffset.z = -baryHeight;
    }
}
