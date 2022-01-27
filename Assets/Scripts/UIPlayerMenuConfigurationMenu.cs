
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UIPlayerMenuConfigurationMenu : MonoBehaviour
{
    public int PlayerIndex;
    public PlayerInputCommands PlayerInputCommands;
    public InputSystemUIInputModule EventSystem;
    public GameObject ReadyPanel;
    public GameObject MenuPanel;


    public void SetPlayerIndex(RectTransform parent)
    {
        PlayerIndex = PlayerInputCommands.PlayerInput.playerIndex;
        transform.SetParent(parent, false);
        PlayerInputCommands.PlayerInput.uiInputModule = EventSystem;
        EventSystem.enabled = true;
        Debug.Log("Panel Setted");
    }

    public void SetColor(Color color)
    {
        MainMenuMultiPlayerManager.Instance.SetPlayerColor(PlayerIndex, color);
    }

    public void SetReadyPlayer()
    {
        MainMenuMultiPlayerManager.Instance.SetPlayReady(PlayerIndex, true);
    }
    
}
