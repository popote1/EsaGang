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
    


    public void SetPlayerIndex(RectTransform parent)
    {
        PlayerIndex = PlayerInputCommands.PlayerInput.playerIndex;
        //MenuPanel.name = "Menu Panel " + PlayerIndex;
        transform.SetParent(parent, false);
        PlayerInputCommands.PlayerInput.uiInputModule = EventSystem;
        EventSystem.enabled = true;
        Debug.Log("Panel Setted");
    }

    public void SetColor(Color color)
    {
        MultiPlayerManager.Instance.SetPlayerColor(PlayerIndex, color);
    }

    public void SetReadyPlayer()
    {
        MultiPlayerManager.Instance.SetPlayReady(PlayerIndex, true);
    }

    public void UIChangeHead(int value)
    {
        IndexHead += value;
        if (IndexHead < 0) IndexHead = HeadSetUps.Length - 1;
        if (IndexHead >= HeadSetUps.Length) IndexHead = 0;
        TxtHeadName.text = HeadSetUps[IndexHead].Name;
    }

    public void UIChangeColor(int value) {
        IndexColor += value;
        if (IndexColor < 0) IndexColor = Colors.Length - 1;
        if (IndexColor >= Colors.Length) IndexColor = 0;
        ImgColor.color = Colors[IndexColor]+new Color(0,0,0,1);
    }

    public void UICangeTeam(int value)
    {
        IndexTeam += value;
        if (IndexTeam < 0) IndexTeam = TeamSetUps.Length - 1;
        if (IndexTeam >= TeamSetUps.Length) IndexTeam = 0;
        ImgTeamColor.color = TeamSetUps[IndexTeam].Color+new Color(0,0,0,1);
        TxtTeamName.text = "TEAM :\r"+TeamSetUps[IndexTeam].Name;
    }

    public void UISetPlayerReader()
    {
        ReadyPanel.SetActive(true);
    }
}

[Serializable]
public class TeamSetUp
{
    public string Name;
    public Color Color;
}

[Serializable]
public class HeadSetUp
{
    public string Name;
    public GameObject prefabs;
}
