using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuScripte : MonoBehaviour
{
    public PlayerMenuInfo[] PlayerMenuInfo;


    public UIPlayerMenuConfigurationMenu PrefabUIPlayerMenuConfigurationMenu;
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

    public void AddPlayerUI(PlayerInputCommands pc, int index)
    {
        UIPlayerMenuConfigurationMenu ui = Instantiate(PrefabUIPlayerMenuConfigurationMenu);
        ui.PlayerInputCommands = pc;
        pc.ConfigMenu = ui;
        ui.SetPlayerIndex(PlayerPanel[index]);
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
