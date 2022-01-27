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
                
                
            }
            
            baryX = baryX / playersInGame.Count;
            baryZ = baryZ / playersInGame.Count;
            
            barycentric.x = baryX;
            barycentric.z = baryZ;

        }

        float baryHeight = (-baryX) * (-baryZ);
        //Debug.Log("BaryX vaut " + baryX+ ", BaryZ vaut " + baryZ) ;
        Debug.Log(baryHeight);
        baryHeight = Mathf.Clamp(baryHeight, 5f, 30f);

        testBary.transform.position = barycentric;

        //transform.position = testBary.transform.position;

        (componentBase as CinemachineFramingTransposer).m_CameraDistance = baryHeight;
        (componentBase as CinemachineFramingTransposer).m_TrackedObjectOffset.y = baryHeight;
    }
}
