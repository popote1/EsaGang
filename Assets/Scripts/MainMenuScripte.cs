using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScripte : MonoBehaviour
{
    public PlayerMenuInfo[] PlayerMenuInfo;


    public RectTransform[] PlayerPanel;
    public GameObject PanelIntro;
    public GameObject PanelSelectionPlayer;


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
