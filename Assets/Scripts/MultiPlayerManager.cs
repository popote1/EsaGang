using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiPlayerManager : MonoBehaviour
{

    public List<MenuPlayerConfiguration> _playerConfigurations;
    

    public int maxplayer = 4;
    public bool IsReadyToLounch;
    public MainMenuScripte MainMenuScripte;
   
    
    public static  MultiPlayerManager Instance { get; private set;}

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log(" SINGELTHON FAUX PAS FAIRE UNE DEUXIEME INSTANCE D'UN SINGELTHON");
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(Instance);
            _playerConfigurations = new List<MenuPlayerConfiguration>();
        }
    }

    public void SetPlayerColor(int index, Color color)
    {
        _playerConfigurations[index].ColorPlayer = color;
    }

    public void SetPlayReady(int index, bool value)
    {
        _playerConfigurations[index].PlayerIsReady = value;
        if (_playerConfigurations.Count > 1 && _playerConfigurations.All(p => p.PlayerIsReady == true)) {
            IsReadyToLounch = true;
        }
    }

    public void AddPlayer(PlayerInput pi)
    {
        Debug.Log("Add a player");

        if (_playerConfigurations.All(p => p.PlayerIndex != pi.playerIndex))
        {
            PlayerInputCommands pc = pi.GetComponent<PlayerInputCommands>();
            _playerConfigurations.Add(new MenuPlayerConfiguration(pc));
            MainMenuScripte.AddPlayerUI(pc, _playerConfigurations.Count-1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
