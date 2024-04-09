using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnakeInGameUIController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider SFXSlider;
    public TextMeshProUGUI resumeText;
    public Snake snake;
    public Boolean isRunning = true;

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
        Time.timeScale = 0;
        isRunning = false;
    }

    public void ResumeGame()
    {
        isRunning = true;
        if (!snake.isGameOver)
        {
            Time.timeScale = 0;
            StartCoroutine(CountingBeforeStart());
        }
    }
    public void QuitGame()
    {
        AudioManager.Instance.PlayMusic("BGM");
        SceneManager.LoadScene("MainMenu");
        
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the scene to restart the game
    }

    private IEnumerator CountingBeforeStart()
    {
        int count = 3; // Reset count to its initial value
        resumeText.gameObject.SetActive(true); // Make sure the text is visible
        while (count > 0)
        {
            resumeText.text = count.ToString();
            Debug.Log("Countdown: " + count); // Verify the countdown is running
            yield return new WaitForSecondsRealtime(1); // Now it will wait for 1 real-time second, regardless of Time.timeScale
            count--;
        }

        resumeText.text = "Start!";
        yield return new WaitForSecondsRealtime(1); // Additional wait to show "Start!"

        Debug.Log("Countdown finished"); // Confirm the coroutine reaches the end
        resumeText.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void PressToStart()
    {
        Time.timeScale = 1;
    }
    public void PlayButtonSFX()
    {
        AudioManager.Instance.PlaySFX("Button");
    }
}
