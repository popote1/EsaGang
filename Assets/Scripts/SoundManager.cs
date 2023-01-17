using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioListener))]
public class SoundManager : MonoBehaviour
{
    
    public static SoundManager Instance { get; private set; }

    public AudioSource MusicAudioSource;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log(" SINGELTHON FAUX PAS FAIRE UNE DEUXIEME INSTANCE D'UN SINGELTHON");
        }
        else
        {
            Instance = this;
            MusicAudioSource = gameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(Instance);
        }
    }
    void Start()
    {
        
    }

    public void PlayerSound(AudioClip clip, float volume)
    {
        AudioSource audioSources = gameObject.AddComponent<AudioSource>();
        audioSources.clip = clip;
        audioSources.volume = volume;
        audioSources.Play();
        Destroy(audioSources, clip.length);
    }

    public void PlayMusic(AudioClip clip, float volume)
    {
        MusicAudioSource.Stop();
        MusicAudioSource.clip = clip;
        MusicAudioSource.volume = volume;
        MusicAudioSource.loop = true;
        MusicAudioSource.Play();
    }
}
