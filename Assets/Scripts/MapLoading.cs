using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class MapLoading : MonoBehaviour
{
    public VeryController3 PrefabsPlayer;
    public Transform[] PlayerSpawn;
    public Color[] TeamsColor;
    public InGamePlayerInfo2[] PlayerPanel;
    public CameraScript CameraScript;
    public CinemachineTargetGroup CinemachineTargetGroup;
    public AudioClip Music;
    [Range(0, 1)] public float MusicVolume=1;


    public float GameTime=60;
    public bool GameIsEnd;
    public float TimerToWinScreen = 4;
    public Text TxtGameTimer;

    [Header("SlowMode Parametres")] public float SlowModeTime=10f;
    public float SlowFactor=0.2f;

    private float _timer;
    private float _gameTimer=0;
    void Start()
    {
        if (MultiPlayerManager.Instance == null) return;

        CinemachineTargetGroup.m_Targets =
            new CinemachineTargetGroup.Target[MultiPlayerManager.Instance._playerConfigurations.Count];
        
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
            //CameraScript.AddPlayerToList(player.gameObject);
            CinemachineTargetGroup.m_Targets[i].weight = 1;
            CinemachineTargetGroup.m_Targets[i].radius = 2;
            CinemachineTargetGroup.m_Targets[i].target = player.transform;
            
            
            PlayerPanel[i].gameObject.SetActive(true);
            PlayerPanel[i].SetFaceColor(MultiPlayerManager.Instance._playerConfigurations[i].ColorPlayer);
            
            if (Music!=null)SoundManager.Instance.PlayMusic(Music, MusicVolume);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameIsEnd)
        {
            _timer += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(1, SlowFactor,  _timer / TimerToWinScreen);
            Debug.Log("Timer = " + Mathf.RoundToInt(_timer));
            if (_timer > TimerToWinScreen)
            {
                MultiPlayerManager.Instance.GoToWinScene();
                Time.timeScale = 1;
            }
            
        }

        if (!GameIsEnd)
        {
            _gameTimer += Time.deltaTime;
            TxtGameTimer.text = Mathf.FloorToInt(GameTime - _gameTimer) + "";
            if (_gameTimer > GameTime) {
                GameIsEnd = true;
            }
        }
    }

    public void PlayerLostHP(MenuPlayerConfiguration player, int damage)
    {
        player.HP += damage;
        PlayerPanel[player.IndexInGamePanel].TakeDamage(damage);
        if (!player.IsAlive)
        {
            Debug.Log(" retire le joueur du baricentre");
            CinemachineTargetGroup.RemoveMember(player.PlayerInputCommands.Player.transform);
        }

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
           // MultiPlayerManager.Instance.GoToWinScene();
        }
    } 
}

