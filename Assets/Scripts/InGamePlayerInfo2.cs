using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InGamePlayerInfo2 : MonoBehaviour
{
    [Header("UIParte;")]
    public Slider SliderHP;
    public Image ImgSliderFile;
    public Image ImgHeadGreyScale;
    public Image ImgHead;

    [Header("Settings")] 
    public Gradient LifeGradiant;
    public Sprite[] GrayScaleSprites;
    public Sprite[] FaceScaleSprites;
    public AnimationCurve HeathCuve = AnimationCurve.Linear(0,0,1,1);
    public float AnimationTime=1;

    private float _currentHP=10;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("TakeDamage")]
    private void TestTakeDamage()
    {
        TakeDamage(-2);
    }
    [ContextMenu("SetColor")]
    private void TestSetFaceColor()
    {
        SetFaceColor( Color.blue);
    }
    public void TakeDamage(float damage )
    {
        
        _currentHP += damage;
        if (_currentHP < 0)
        {
            _currentHP = 0;
            return;
        }
        SliderHP.DOPause();
        SliderHP.DOValue(_currentHP / 10, AnimationTime).SetEase(HeathCuve);
        ImgSliderFile.DOPause();
        ImgSliderFile.DOColor(LifeGradiant.Evaluate(_currentHP / 10), AnimationTime).SetEase(HeathCuve);
        if (_currentHP > 7)
        {
            ImgHead.sprite = FaceScaleSprites[0];
            ImgHeadGreyScale.sprite=GrayScaleSprites[0];
        }
        else if (_currentHP > 5)
        {
            ImgHead.sprite = FaceScaleSprites[1];
            ImgHeadGreyScale.sprite=GrayScaleSprites[1];
        }else if (_currentHP > 2)
        {
            ImgHead.sprite = FaceScaleSprites[2];
            ImgHeadGreyScale.sprite=GrayScaleSprites[2];
        }
        else if (_currentHP ==0)
        {
            ImgHead.sprite = FaceScaleSprites[3];
            ImgHeadGreyScale.sprite=GrayScaleSprites[3];
        }
    }

    public void SetFaceColor(Color color)
    {
        ImgHeadGreyScale.color = color+new Color(0,0,0,1);
    }
}
