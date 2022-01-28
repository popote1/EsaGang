using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{

    public Text TimerText;
    public float TimeOnScene;
    public AudioClip Music;
    [Range(0, 1)] public float MusicVolume=1;

    private float _timer;
    void Start()
    {
        if (MultiPlayerManager.Instance != null)
        {
            _timer += TimeOnScene;
            if (Music!=null)SoundManager.Instance.PlayMusic(Music, MusicVolume);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        TimerText.text = Mathf.FloorToInt(_timer) + "S";
        if (_timer <= 0)
        {
            MultiPlayerManager.Instance.ReturnToMainMenu();
        }
    }
}
