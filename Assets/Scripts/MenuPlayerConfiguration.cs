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

    
    public bool IsAlive = true;
    public int DeadIndex;
    public int IndexInGamePanel;

    public int HP
    {
        get => _hp;
        set {
             _hp = value;
            if (_hp <= 0) {
                IsAlive = false;
                _hp = 0;
            }
        }
    }

    private int _hp;


    public MenuPlayerConfiguration(PlayerInputCommands playerInputCommands)
    {
        PlayerInputCommands = playerInputCommands;
        PlayerIndex = playerInputCommands.PlayerInput.playerIndex;
    }

    public void SetValues(int head, Color color, int team)
    {
        HeadIndex = head;
        FactionIndex = team;
        ColorPlayer = color;
    }
    

}
