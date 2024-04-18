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
    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVol", 0.75f); // Default value if not set
        bgmSlider.value = PlayerPrefs.GetFloat("bgmVol", 0.75f);
        SFXSlider.value = PlayerPrefs.GetFloat("sfxVol", 0.75f);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        SFXSlider.onValueChanged.AddListener(SetEffectVolume);

        // Apply the initial volume settings
        SetMasterVolume(masterSlider.value);
        SetBgmVolume(bgmSlider.value);
        SetEffectVolume(SFXSlider.value);
    }

    // This function will control the master volume
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", LinearToDecibel(volume));
        PlayerPrefs.SetFloat("masterVol", volume);
    }

    public void SetBgmVolume(float volume)
    {
        audioMixer.SetFloat("bgmVolume", LinearToDecibel(volume));
        PlayerPrefs.SetFloat("bgmVol", volume);
    }

    public void SetEffectVolume(float volume)
    {
        audioMixer.SetFloat("SoundEffect", LinearToDecibel(volume));
        PlayerPrefs.SetFloat("sfxVol", volume);
    }

    // Convert linear scale [0,1] to decibel scale [-80,0]
    private float LinearToDecibel(float linear)
    {
        return linear != 0 ? 20.0f * Mathf.Log10(linear) : -80.0f;
    }

    public void PauseGame()
    {

    }

    public void ResumeGame()
    {

    }
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}