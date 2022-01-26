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
    private float baryHeight;

    public CinemachineVirtualCamera virtualCam;
    private CinemachineComponentBase componentBase;
    
    [SerializeField] GameObject farthestPlayerX1, farthestPlayerX2, farthestPlayerZ1, farthestPlayerZ2;

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
        if (farthestPlayerX1 == null)
        {
            farthestPlayerX1 = gameObject;
        }

        if (farthestPlayerX2 == null)
        {
            farthestPlayerX2 = gameObject;
        }
        if (farthestPlayerZ1 == null)
        {
            farthestPlayerZ1 = gameObject;
        }

        if (farthestPlayerZ2 == null)
        {
            farthestPlayerZ2 = gameObject;
        }
    }

    private void FixedUpdate()
    {
        if (playersInGame.Count != 0)
        {
            foreach (GameObject gameObject in playersInGame)
            {
                baryX += gameObject.transform.position.x;
                baryZ += gameObject.transform.position.z;
                
                if (gameObject.transform.position.x < farthestPlayerX1.transform.position.x)
                {
                    farthestPlayerX1 = gameObject;
                }

                if (gameObject.transform.position.x > farthestPlayerX2.transform.position.x)
                {
                    farthestPlayerX2 = gameObject;
                }
                if (gameObject.transform.position.z < farthestPlayerZ1.transform.position.z)
                {
                    farthestPlayerX1 = gameObject;
                }

                if (gameObject.transform.position.z > farthestPlayerZ2.transform.position.z)
                {
                    farthestPlayerX2 = gameObject;
                }
            }
            
            baryX = baryX / playersInGame.Count;
            baryZ = baryZ / playersInGame.Count;
            
            barycentric.x = baryX;
            barycentric.z = baryZ;
            
        }
        
        float distanceFarthestZ = Vector3.Magnitude(farthestPlayerZ1.transform.position - farthestPlayerZ2.transform.position); 
        float distanceFarthestX = Vector3.Magnitude(farthestPlayerX1.transform.position - farthestPlayerX2.transform.position);
        baryHeight = distanceFarthestX + distanceFarthestZ;
        
        Debug.Log(baryHeight);
        baryHeight = Mathf.Clamp(baryHeight, 5f, 30f);

        testBary.transform.position = barycentric;

        //transform.position = testBary.transform.position;

        (componentBase as CinemachineFramingTransposer).m_CameraDistance = baryHeight;
        (componentBase as CinemachineFramingTransposer).m_TrackedObjectOffset.y = baryHeight;
        (componentBase as CinemachineFramingTransposer).m_TrackedObjectOffset.z = -baryHeight;
    }
}
