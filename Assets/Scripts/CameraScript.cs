using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public List<GameObject> playersInGame = new List<GameObject>();
    public float baryX, baryZ;
    public float minZoom, maxZoom;
    public Vector3 barycentric;
    
    public GameObject testBary;
    private void Awake()
    {
    }

    void Start()
    {
        
    }

    public void AddPlayerToList(GameObject player)
    {
        playersInGame.Add(player);
    }

    private void FixedUpdate()
    {
        Debug.Log(playersInGame.Count);
        if (playersInGame.Count != 0)
        {
            foreach (GameObject gameObject in playersInGame)
            {
                baryX += gameObject.transform.position.x;
                baryZ += gameObject.transform.position.z;
                
                baryX = baryX / playersInGame.Count;
                baryZ = baryZ / playersInGame.Count;
            }
            
            barycentric.x = baryX;
            barycentric.z = baryZ;
            
        }

        testBary.transform.position = barycentric;

        transform.position = testBary.transform.position;
    }
}
