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
    
    private void Start()
    {
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();

        //SceneManager.sceneLoaded += SceneLoaded;
    }

    //private void SceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (SceneManager.GetActiveScene().name == "MainMenu")
    //    {
    //        Debug.Log("running");

    //        float value;

    //        audioMixer.GetFloat("MasterVol", out value);
    //        Debug.Log(value);
    //        value = Mathf.Pow(10, value / 20);
    //        Debug.Log(value);
    //        masterSlider.value = value;
    //        Debug.Log(masterSlider.value);

    //        audioMixer.GetFloat("MusicVol", out value);
    //        value = Mathf.Pow(10, value / 20);

    //        musicSlider.value = value;

    //        audioMixer.GetFloat("SFXVol", out value);
    //        value = Mathf.Pow(10, value / 20);
    //        SFXSlider.value = value;
    //    }
    //}

    public void SetMasterVolume()
    {
        if (masterSlider && audioMixer)
        {
            float volume = masterSlider.value;
            audioMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
        }

    }

    public void SetMusicVolume()
    {
        if (masterSlider && audioMixer)
        {
            float volume = musicSlider.value;
            audioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
        }
        
    }

    public void SetSFXVolume()
    {
        if (masterSlider && audioMixer)
        {
            float volume = SFXSlider.value;
            audioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        }
    }


}
