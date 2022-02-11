using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{

    public Text TimerText;
    public float TimeOnScene;
    public AudioClip Music;
    [Range(0, 1)] public float MusicVolume=1;

    public Body[] WinnersBodys;
    public Body[] LoserBodys;

    public GameObject[] Heads;
    public TeamSetUp[] TeamSetUps;

    private float _timer;

    void Start()
    {
        if (MultiPlayerManager.Instance != null)
        {
            _timer += TimeOnScene;
            if (Music != null) {
                SoundManager.Instance.PlayMusic(Music, MusicVolume);
            }

            for (int i = 0; i < MultiPlayerManager.Instance._playerConfigurations.Count; i++)
            {
                if (MultiPlayerManager.Instance._playerConfigurations[i].IsAlive)
                {
                    
                }



                    VeryController3 player =
                        Instantiate(PrefabsPlayer, PlayerSpawn[i].position, PlayerSpawn[i].rotation);
                MultiPlayerManager.Instance._playerConfigurations[i].PlayerInputCommands.Player = player;
                player.PlayerInfo = MultiPlayerManager.Instance._playerConfigurations[i];
                player.MapLoading = this;
                player.PlayerInputCommands = MultiPlayerManager.Instance._playerConfigurations[i].PlayerInputCommands;

                MultiPlayerManager.Instance._playerConfigurations[i].HP = 10;
                MultiPlayerManager.Instance._playerConfigurations[i].IndexInGamePanel = i;
                player.PlayerMeshRenderer.material.color =
                    MultiPlayerManager.Instance._playerConfigurations[i].ColorPlayer;
                player.TchirtMeshRenderer.material.color =
                    TeamsColor[MultiPlayerManager.Instance._playerConfigurations[i].FactionIndex];
                //CameraScript.AddPlayerToList(player.gameObject);
                if (Music != null) SoundManager.Instance.PlayMusic(Music, MusicVolume);
            }
        }
    }

    private void SetWinnerBody(MenuPlayerConfiguration info)
    {
        Body body = WinnersBodys.Where(w => w.FullBody.activeSelf).ToArray()[0];
        body.BodyMeshRenderer.material.color = info.ColorPlayer;
        body.TchirtMeshRenderer.material.color = TeamSetUps[info.FactionIndex].Color;
        
        GameObject head =Instantiate(Heads[info.HeadIndex], body.Head);
        head.transform.position = body.Head.position;
        head.transform.rotation = body.Head.rotation;

    }


    // Update is called once per frame
    void Update() {
        _timer -= Time.deltaTime;
        TimerText.text = Mathf.FloorToInt(_timer) + "S";
        if (_timer <= 0) {
            MultiPlayerManager.Instance.ReturnToMainMenu();
        }
    }
}
 
[Serializable]
public struct Body
{
    public GameObject FullBody;
    public Transform Head;
    public MeshRenderer BodyMeshRenderer;
    public MeshRenderer TchirtMeshRenderer;
}
