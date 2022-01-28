using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class MainMenuScripte : MonoBehaviour
{
    
    public UIPlayerMenuConfigurationMenu PrefabUIPlayerMenuConfigurationMenu;
    public MultiPlayerManager Prefabs;
    public RectTransform[] PlayerPanel;
    public GameObject PanelIntro;
    public GameObject PanelSelectionPlayer;
    [Header("MapSelector")]
    public MapDate[] MapDates;
    public GameObject MapPanel;
    public InputSystemUIInputModule MapEventSysteme;
    public Image ImgLevel;
    public Text TxtLevel;

    private int _indexmap;


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

    public void loadLevelPannel()
    {
        MapPanel.SetActive(true);
        foreach (MenuPlayerConfiguration player in MultiPlayerManager.Instance._playerConfigurations)
        {
            player.PlayerInputCommands.ConfigMenu.EventSystem.enabled = false;
        }
        MultiPlayerManager.Instance._playerConfigurations[0].PlayerInputCommands.PlayerInput.uiInputModule = MapEventSysteme;
        ImgLevel.sprite = MapDates[0].Sprie;
        TxtLevel.text = MapDates[0].Name;
        _indexmap = 0;
    }

    public void UIChangeLevel(int value)
    {
        _indexmap += value;
        if (_indexmap < 0) _indexmap = MapDates.Length - 1;
        if (_indexmap >=MapDates.Length) _indexmap = 0;
        ImgLevel.sprite = MapDates[_indexmap].Sprie;
        TxtLevel.text = MapDates[_indexmap].Name;
    }

    public void loadGame()
    {
        MultiPlayerManager.Instance.LoadNewScene(MapDates[_indexmap].MapSceneIndex);
    }
}

[Serializable]
public struct MapDate
{
    public string Name;
    public Sprite Sprie;
    public int MapSceneIndex;
    
}
