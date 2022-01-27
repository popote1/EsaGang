using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScripte : MonoBehaviour
{
    public PlayerMenuInfo[] PlayerMenuInfo;


    public MultiPlayerManager Prefabs;
    public RectTransform[] PlayerPanel;
    public GameObject PanelIntro;
    public GameObject PanelSelectionPlayer;
    

    private void Start()
    {
        if (MultiPlayerManager.Instance == null)
        {
            Instantiate(Prefabs);
        }
        MultiPlayerManager.Instance.MainMenuScripte = this;
    }

    

    public void UIEntreSelection()
    {
        PanelIntro.SetActive(false);
        PanelSelectionPlayer.SetActive(true);
    }

    public void UIEnterIntro()
    {
        PanelIntro.SetActive(true);
        PanelSelectionPlayer.SetActive(false);
    }
}
