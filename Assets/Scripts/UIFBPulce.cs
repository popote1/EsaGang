using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIFBPulce : MonoBehaviour
{
   public float PulceFrequancy = 1;
   public float ScaleFactor = 1.5f;
   public AnimationCurve PulceCurve = AnimationCurve.Linear(0,0,1,1);

   public void Start() {
      transform.DOScale(Vector3.one * ScaleFactor, PulceFrequancy).SetLoops(-1, LoopType.Yoyo).SetEase(PulceCurve);
   }
}
