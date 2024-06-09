using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

//class to adjust volumes of audio on change in audio UI
public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    
    // set volume based on save setting or UI setting
    private void Start()
    {
        if(PlayerPrefs.HasKey("MasterVol"))
        {
            LoadMasterVolume();
        }
        else
        {
            SetMasterVolume();
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("SFXVol"))
        {
            LoadSFXVolume();
        }
        else
        {
            SetSFXVolume();
        }
    }

    // update volume based in UI slider value - save
    public void SetMasterVolume()
    {
        if (masterSlider && audioMixer)
        {
            float volume = masterSlider.value;
            audioMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("MasterVol", volume);
        }
    }
    // update slider to match saved value - then update volume
    private void LoadMasterVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVol");

        SetMasterVolume();
    }


    // update volume based in UI slider value - save
    public void SetMusicVolume()
    {
        if (musicSlider && audioMixer)
        {
            float volume = musicSlider.value;
            audioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("MusicVol", volume);
        }

    }
    // update slider to match saved value - then update volume
    private void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol");

        SetMusicVolume();
    }


    // update volume based in UI slider value - save
    public void SetSFXVolume()
    {
        if (SFXSlider && audioMixer)
        {
            float volume = SFXSlider.value;
            audioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SFXVol", volume);
        }
    }
    // update slider to match saved value - then update volume
    private void LoadSFXVolume()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVol");

        SetSFXVolume();
    }
}
