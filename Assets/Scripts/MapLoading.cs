using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoading : MonoBehaviour
{
    public VeryController3 PrefabsPlayer;
    public Transform[] PlayerSpawn;
    public Color[] TeamsColor;
    public InGamePlayerInfo2[] PlayerPanel;
    public CameraScript CameraScript;

    public bool GameIsEnd;
    void Start()
    {
        if (MultiPlayerManager.Instance == null) return;

        
        for(int i=0; i< MultiPlayerManager.Instance._playerConfigurations.Count;i++)
        {
            VeryController3 player =Instantiate(PrefabsPlayer, PlayerSpawn[i].position, PlayerSpawn[i].rotation);
            MultiPlayerManager.Instance._playerConfigurations[i].PlayerInputCommands.Player = player;
            player.PlayerInfo = MultiPlayerManager.Instance._playerConfigurations[i];
            player.MapLoading = this;
            player.PlayerInputCommands = MultiPlayerManager.Instance._playerConfigurations[i].PlayerInputCommands;

            MultiPlayerManager.Instance._playerConfigurations[i].HP = 10;
            MultiPlayerManager.Instance._playerConfigurations[i].IndexInGamePanel= i;
            player.PlayerMeshRenderer.material.color = MultiPlayerManager.Instance._playerConfigurations[i].ColorPlayer;
            player.TchirtMeshRenderer.material.color = TeamsColor[MultiPlayerManager.Instance._playerConfigurations[i].FactionIndex];
            CameraScript.AddPlayerToList(player.gameObject);
            
            PlayerPanel[i].gameObject.SetActive(true);
            PlayerPanel[i].SetFaceColor(MultiPlayerManager.Instance._playerConfigurations[i].ColorPlayer);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerLostHP(MenuPlayerConfiguration player, int damage)
    {
        player.HP += damage;
        PlayerPanel[player.IndexInGamePanel].TakeDamage(damage);
        CheckGameState();
    }

    public void CheckGameState()
    {
        List<int> teamAlive = new List<int>();
        foreach (MenuPlayerConfiguration player in MultiPlayerManager.Instance._playerConfigurations) {
            if (player.IsAlive&&!teamAlive.Contains(player.FactionIndex)) {
                teamAlive.Add(player.FactionIndex);
            }
        }

        if (teamAlive.Count == 1)
        {
            GameIsEnd = true;
            MultiPlayerManager.Instance.GoToWinScene();
        }
    } 
}

