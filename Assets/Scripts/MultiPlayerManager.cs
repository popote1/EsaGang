using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
            DontDestroyOnLoad(Instance);
            _playerConfigurations = new List<MenuPlayerConfiguration>();
        }
    }

    public void SetPlayerInfo(int index, int head, Color color, int team) {
        _playerConfigurations[index].SetValues(head, color, team);
    }
    
    public void SetPlayReady(int index, bool value)
    {
        _playerConfigurations[index].PlayerIsReady = value;
        if (_playerConfigurations.Count > 1 && _playerConfigurations.All(p => p.PlayerIsReady == true)) {
            IsReadyToLounch = true;
            LoadnewScene(1);
        }
    }

    public void AddPlayer(PlayerInput pi)
    {
        Debug.Log("Add a player");

        if (_playerConfigurations.All(p => p.PlayerIndex != pi.playerIndex))
        {
            PlayerInputCommands pc = pi.GetComponent<PlayerInputCommands>();
            _playerConfigurations.Add(new MenuPlayerConfiguration(pc));
            pc.transform.SetParent(transform);
            MainMenuScripte.AddPlayerUI(pc, _playerConfigurations.Count-1);
        }
    }

    private void LoadnewScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
