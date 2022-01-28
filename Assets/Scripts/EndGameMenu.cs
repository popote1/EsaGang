using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{

    public Text TimerText;
    public float TimeOnScene;

    private float _timer;
    void Start()
    {
        if (MultiPlayerManager.Instance != null)
        {
            _timer += TimeOnScene;
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
