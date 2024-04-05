using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider SFXSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("masterVol") || PlayerPrefs.HasKey("bgmVol") || PlayerPrefs.HasKey("sfxVol"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
            SetBgmVolume();
            SetEffectVolume();
        }
    }

    // This function will control the master volume
    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("masterVol", volume);
    }
    public void SetBgmVolume()
    {
        float volume = bgmSlider.value;
        audioMixer.SetFloat("bgmVolume", volume);
        PlayerPrefs.SetFloat("bgmVol", volume);
    }

    public void SetEffectVolume()
    {
        float volume = SFXSlider.value;
        audioMixer.SetFloat("SoundEffect", volume);
        PlayerPrefs.SetFloat("sfxVol", volume);
    }

    private void LoadVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVol");
        bgmSlider.value = PlayerPrefs.GetFloat("bgmVol");
        SFXSlider.value = PlayerPrefs.GetFloat("sfxVol");

        SetMasterVolume();
        SetBgmVolume();
        SetEffectVolume();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {

    }
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
