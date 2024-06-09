using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private Sound musicSound;
    [SerializeField] private Sound[] SFXSounds;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (musicSound != null)
        {
            musicSource.clip = musicSound.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound soundToPlay = Array.Find(SFXSounds, element => element.soundName == name);
        if (soundToPlay == null)
        {
            Debug.Log("No sound found");
        }
        else
        {
            SFXSource.clip = soundToPlay.clip;
            SFXSource.Play();
        }
    }


}
