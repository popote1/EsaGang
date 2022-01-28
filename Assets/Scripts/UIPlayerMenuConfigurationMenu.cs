using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class UIPlayerMenuConfigurationMenu : MonoBehaviour
{
    public int PlayerIndex;
    public PlayerInputCommands PlayerInputCommands;
    public InputSystemUIInputModule EventSystem;
    public GameObject ReadyPanel;
    public GameObject MenuPanel;
    public AudioClip EnterSound;
    [Range(0, 1)] public float EntersoundVolume=1;
    [Header("Heade")] 
    public int IndexHead=0;
    public HeadSetUp[] HeadSetUps;
    public Text TxtHeadName;
    [Header("Color")] 
    public int IndexColor=0;
    public Color[] Colors;
    public Image ImgColor;
    [Header("Team")]
    public int IndexTeam=0;
    public TeamSetUp[] TeamSetUps;
    public Text TxtTeamName;
    public Image ImgTeamColor;
    [Header("ExtraModels")] 
    public SkinnedMeshRenderer PlayerBody;
    public SkinnedMeshRenderer PlayerTChirt;
    public SpriteRenderer TeamSprite;
    public Transform Head;

    private GameObject _head;
    public bool CanInteract {
        get {return EventSystem.enabled;
        }
        set {
            EventSystem.enabled = CanInteract;
        }
    }

    public void SetPlayerIndex(RectTransform parent)
    {
        PlayerIndex = PlayerInputCommands.PlayerInput.playerIndex;
        //MenuPanel.name = "Menu Panel " + PlayerIndex;
        transform.SetParent(parent, false);
        PlayerInputCommands.PlayerInput.uiInputModule = EventSystem;
        EventSystem.enabled = true;
        Debug.Log("Panel Setted");
        
        
        TxtHeadName.text = HeadSetUps[IndexHead].Name;
        ImgColor.color = Colors[IndexColor]+new Color(0,0,0,1);
        ImgTeamColor.color = TeamSetUps[IndexTeam].Color+new Color(0,0,0,1);
        TxtTeamName.text = "TEAM :\r"+TeamSetUps[IndexTeam].Name;
        TeamSprite.sprite = TeamSetUps[IndexTeam].Sprite;
        PlayerTChirt.material.color = TeamSetUps[IndexTeam].Color + new Color(0, 0, 0, 1);
        PlayerBody.material.color = Colors[IndexColor]+new Color(0,0,0,1);

        _head = Instantiate(HeadSetUps[0].prefabs, Head.position, Head.rotation);
        _head.transform.SetParent(Head);
        _head.transform.localScale = Head.transform.localScale;
        
        
        
        SoundManager.Instance.PlayerSound(EnterSound, EntersoundVolume);
    }
    

    public void SetReadyPlayer()
    {
        MultiPlayerManager.Instance.SetPlayerInfo(PlayerIndex , IndexHead, Colors[IndexColor], IndexTeam);
        MultiPlayerManager.Instance.SetPlayReady(PlayerIndex, true);
    }

    public void UIChangeHead(int value)
    {
        IndexHead += value;
        if (IndexHead < 0) IndexHead = HeadSetUps.Length - 1;
        if (IndexHead >= HeadSetUps.Length) IndexHead = 0;
        TxtHeadName.text = HeadSetUps[IndexHead].Name;
        
        Destroy(_head);
        _head = Instantiate(HeadSetUps[IndexHead].prefabs, Head.position, Head.rotation);
        _head.transform.SetParent(Head);
        _head.transform.localScale = Head.transform.localScale;
    }

    public void UIChangeColor(int value) {
        IndexColor += value;
        if (IndexColor < 0) IndexColor = Colors.Length - 1;
        if (IndexColor >= Colors.Length) IndexColor = 0;
        ImgColor.color = Colors[IndexColor]+new Color(0,0,0,1);
        PlayerBody.material.color = Colors[IndexColor]+new Color(0,0,0,1);
    }

    public void UICangeTeam(int value)
    {
        IndexTeam += value;
        if (IndexTeam < 0) IndexTeam = TeamSetUps.Length - 1;
        if (IndexTeam >= TeamSetUps.Length) IndexTeam = 0;
        ImgTeamColor.color = TeamSetUps[IndexTeam].Color+new Color(0,0,0,1);
        TxtTeamName.text = "TEAM :\r"+TeamSetUps[IndexTeam].Name;
        TeamSprite.sprite = TeamSetUps[IndexTeam].Sprite;
        PlayerTChirt.material.color = TeamSetUps[IndexTeam].Color + new Color(0, 0, 0, 1);
    }

    public void UISetPlayerReader()
    {
        ReadyPanel.SetActive(true);
    }

    public void GoBack()
    {
        MultiPlayerManager.Instance._playerConfigurations.Remove(
            MultiPlayerManager.Instance._playerConfigurations[PlayerIndex]);
        Destroy(PlayerInputCommands.gameObject);
        Destroy(gameObject);
    }
}

[Serializable]
public class TeamSetUp
{
    public string Name;
    public Color Color;
    public Sprite Sprite;
}

[Serializable]
public class HeadSetUp
{
    public string Name;
    public GameObject prefabs;
}
