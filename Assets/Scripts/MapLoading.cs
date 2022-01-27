using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoading : MonoBehaviour
{
    public VeryController3 PrefabsPlayer;
    public Transform[] PlayerSpawn;
    public Color[] TeamsColor;
    private
    void Start()
    {
        if (MultiPlayerManager.Instance == null) return;

        for(int i=0; i< MultiPlayerManager.Instance._playerConfigurations.Count;i++)
        {
            VeryController3 player =Instantiate(PrefabsPlayer, PlayerSpawn[i].position, PlayerSpawn[i].rotation);
            MultiPlayerManager.Instance._playerConfigurations[i].PlayerInputCommands.Player = player;
            player.PlayerInputCommands = MultiPlayerManager.Instance._playerConfigurations[i].PlayerInputCommands;

            MultiPlayerManager.Instance._playerConfigurations[i].HP = 10;
            player.PlayerMeshRenderer.material.color = MultiPlayerManager.Instance._playerConfigurations[i].ColorPlayer;
            player.TchirtMeshRenderer.material.color = TeamsColor[MultiPlayerManager.Instance._playerConfigurations[i].FactionIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerLostHP()
    {
        
    }
}

public class playerstat
{
    
}
