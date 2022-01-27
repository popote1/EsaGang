using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class MenuPlayerConfiguration 
{
    
    public PlayerInputCommands PlayerInputCommands;
    public int PlayerIndex;
    public int HeadIndex;
    public int FactionIndex;
    public Color ColorPlayer;
    public bool PlayerIsReady;


    public MenuPlayerConfiguration(PlayerInputCommands playerInputCommands)
    {
        PlayerInputCommands = playerInputCommands;
        PlayerIndex = playerInputCommands.PlayerInput.playerIndex;
    }

}
