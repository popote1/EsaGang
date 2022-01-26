using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject[] PlayersPanel;
    public Transform[] SpawnPoints;
    public VeryController2 PlayerPrefab;
    public List<VeryController2> PlayerList = new List<VeryController2>();
    
    public List<PlayerInputCommands> PlayerInputCommandsList = new List<PlayerInputCommands>();
    public List<PlayerInput> PlayerInputs = new List<PlayerInput>();
    
    void Start()
    {
        foreach (GameObject panel in PlayersPanel) {
            panel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerJoind(PlayerInput playerInput)
    {
        Debug.Log(" Player AddJoind");
        
        PlayerInputs.Add(playerInput);
        PlayerInputCommands command = playerInput.GetComponent<PlayerInputCommands>();
        
        Transform SpawnPonit = SpawnPoints[PlayerList.Count];
        VeryController2 player = Instantiate(PlayerPrefab, SpawnPonit.position, SpawnPonit.rotation);
        player.PlayerInputCommands =command;
        command.Player = player;
        PlayersPanel[PlayerList.Count].SetActive(true);
        
        PlayerList.Add(player);
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        PlayerInputs.Remove(playerInput);
        Debug.Log(" Player leftGame");
    }
}
