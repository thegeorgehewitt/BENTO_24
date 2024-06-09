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

    // ensure only one manager exists
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

    // start music when game loads
    private void Start()
    {
        PlayMusic();
    }

    // start music
    public void PlayMusic()
    {
        if (musicSound != null && musicSource != null)
        {
            musicSource.clip = musicSound.clip;
            musicSource.Play();
        }
    }

    // play sound based on name passed in
    public void PlaySFX(string name)
    {
        Sound soundToPlay = Array.Find(SFXSounds, element => element.soundName == name);
        if (soundToPlay == null)
        {
            Debug.Log("No sound found");
        }
        else if (SFXSource != null)
        {
            SFXSource.clip = soundToPlay.clip;
            SFXSource.Play();
        }
    }


}
