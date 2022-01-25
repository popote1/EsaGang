using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public List<GameObject> playersInGame = new List<GameObject>();
    private void Awake()
    {
    }

    void Start()
    {
        
    }

    public void AddPlayerToList()
    {
        playersInGame.Add(GameObject.FindGameObjectWithTag("Player"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
