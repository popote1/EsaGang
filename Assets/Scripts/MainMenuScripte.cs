using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering.UI;

public class MainMenuScripte : MonoBehaviour
{
    
    public UIPlayerMenuConfigurationMenu PrefabUIPlayerMenuConfigurationMenu;
    public MultiPlayerManager Prefabs;
    public SoundManager PrefabsSoundManager;
    public RectTransform[] PlayerPanel;
    public GameObject[] PressToEnter;
    public GameObject PanelIntro;
    public GameObject PanelSelectionPlayer;
    public AudioClip Music;
    [Range(0, 1)] public float MusicVolume=1;
    [Header("MapSelector")]
    public AudioClip MapOpenSound;
    [Range(0, 1)] public float MapOpenSoundVolume=1;
    public MapDate[] MapDates;
    public GameObject MapPanel;
    public InputSystemUIInputModule MapEventSysteme;
    public Image ImgLevel;
    public Text TxtLevel;

    
    private int _indexmap;


    private void Start()
    {
        if (MultiPlayerManager.Instance == null) Instantiate(Prefabs);
        if (SoundManager.Instance == null) Instantiate(PrefabsSoundManager);
        if (Music!=null)SoundManager.Instance.PlayMusic(Music, MusicVolume);
        MultiPlayerManager.Instance.MainMenuScripte = this;
    }

    public void AddPlayerUI(PlayerInputCommands pc, int index)
    {
        int playerSlote = MultiPlayerManager.Instance.GetFreeSlote();
        if (playerSlote ==-1) Debug.LogWarning( "No Player Formor players");
        
        UIPlayerMenuConfigurationMenu ui = Instantiate(PrefabUIPlayerMenuConfigurationMenu);
        ui.PlayerInputCommands = pc;
        pc.ConfigMenu = ui;
        ui.Slote = playerSlote;
        MultiPlayerManager.Instance.Slots[playerSlote] = ui;
        ui.SetPlayerIndex(PlayerPanel[playerSlote]);
        PressToEnter[playerSlote].SetActive(false);
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
        if (Music!=null)SoundManager.Instance.PlayerSound(MapOpenSound,MapOpenSoundVolume);
        MapPanel.SetActive(true);
        //foreach (MenuPlayerConfiguration player in MultiPlayerManager.Instance._playerConfigurations)
        //{
        //    player.PlayerInputCommands.ConfigMenu.EventSystem.enabled = false;
        //}
        for (int i = 0; i < MultiPlayerManager.Instance.MenuSlotes.Length; i++)
        {
            if (MultiPlayerManager.Instance.MenuSlotes[i].PlayerInputCommands==null)continue;
            MultiPlayerManager.Instance.MenuSlotes[i].PlayerInputCommands.ConfigMenu.EventSystem.enabled = false;
            MultiPlayerManager.Instance._playerConfigurations.Add(MultiPlayerManager.Instance.MenuSlotes[i]);
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
