using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxComponent : MonoBehaviour
{
    public ParalaxElement[] ParalaxElements;
    public Transform CurrentTarget;

    public void Start()
    {
        for (int i = 0; i < ParalaxElements.Length; i++)
        {
            ParalaxElements[i].OriginalPos = ParalaxElements[i].Object.position;
        }
    }

    public void Update()
    {
        for (int i = 0; i < ParalaxElements.Length; i++)
        {
            ParalaxElements[i].Object.position = Vector3.Lerp(
                ParalaxElements[i].OriginalPos,
                CurrentTarget.position,
                ParalaxElements[i].Facteur);
        }
    }
}

[Serializable]
public struct ParalaxElement
{
    public Transform Object;
    [Range(-1,1)]public float Facteur;
    [NonSerialized]
    public Vector3 OriginalPos;
}
