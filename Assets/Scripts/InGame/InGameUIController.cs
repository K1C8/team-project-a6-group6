using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TextMeshProUGUI resumeText;
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
        Time.timeScale = 0;
        StartCoroutine(CountingBeforeStart());
    }
    public void QuitGame()
    {
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
}
