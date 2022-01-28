using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TranslateScripte : MonoBehaviour
{
    public Vector3 Pos1;
    public Vector3 Pos2;
    public float AnimTime=5;
    
    void Start()
    {
        transform.position = Pos1;
        transform.DOMove(Pos2, AnimTime).SetLoops(-1,LoopType.Yoyo);
    }

   
}
