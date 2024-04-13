using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JetGameManagerLogic : MonoBehaviour 
{
    private int _playerScore;

    [SerializeField] private GameObject _gameOverSingleMode; 
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private Slider _effectVolumeSlider;

    void Start()
    {
        _masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVol", 0.75f); // Default value if not set
        _bgmVolumeSlider.value = PlayerPrefs.GetFloat("bgmVol", 0.75f);
        _effectVolumeSlider.value = PlayerPrefs.GetFloat("sfxVol", 0.75f);

        _masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        _bgmVolumeSlider.onValueChanged.AddListener(SetBgmVolume);
        _effectVolumeSlider.onValueChanged.AddListener(SetEffectVolume);

        // Debug.Log(string.Format("Master Volume: {0}, BGM Volume: {1}, Effect Volume:{2}", _masterVolumeSlider.value, _bgmVolumeSlider.value, _effectVolumeSlider.value));

        // Apply the initial volume settings
        SetMasterVolume(_masterVolumeSlider.value);
        SetBgmVolume(_bgmVolumeSlider.value);
        SetEffectVolume(_effectVolumeSlider.value);
    }

    public void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat("MasterVolume", LinearToDecibel(volume));
        PlayerPrefs.SetFloat("masterVol", volume);
        // Debug.Log(string.Format("Master Volume: {0}", _masterVolumeSlider.value));
    }

    public void SetBgmVolume(float volume)
    {
        _audioMixer.SetFloat("bgmVolume", LinearToDecibel(volume));
        PlayerPrefs.SetFloat("bgmVol", volume);
        // Debug.Log(string.Format("BGM Volume: {0}", _bgmVolumeSlider.value));
    }

    public void SetEffectVolume(float volume)
    {
        _audioMixer.SetFloat("SoundEffect", LinearToDecibel(volume));
        PlayerPrefs.SetFloat("sfxVol", volume);
        // Debug.Log(string.Format("Effect Volume: {0}", _effectVolumeSlider.value));
    }

    private float LinearToDecibel(float linear)
    {
        return linear != 0 ? 20.0f * Mathf.Log10(linear) : -80.0f;
    }
    public void PlayButtonSFX()
    {
        AudioManager.Instance.PlaySFX("Button");
    }


    public void Pause()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int PlayerScore
    {
        get { return _playerScore; }
        set { _playerScore = value; }
    }

    // Member to call when user touches a button
    public void PressToStart()
    {
        JetPlayerController JetPlayer = (FindObjectsByType<JetPlayerController>(FindObjectsInactive.Include, FindObjectsSortMode.None))[0];
        EnemySpawnManager EnemySpawnManager = (FindObjectsByType<EnemySpawnManager>(FindObjectsInactive.Include, FindObjectsSortMode.None))[0];
        GameObject pressToStartText = GameObject.Find("PressToStart");
        if (JetPlayer != null && EnemySpawnManager != null && pressToStartText != null)
        {
            JetPlayer.gameObject.SetActive(true);
            EnemySpawnManager.gameObject.SetActive(true);
            pressToStartText.SetActive(false);
        }

    }

    IEnumerator GameOverProcess()
    {
        yield return new WaitForSeconds(3f);
        _gameOverSingleMode.SetActive(true);
    }

    public void OnGameOver()
    {
        StartCoroutine(GameOverProcess());
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlayMusic("BGM");
        SceneManager.LoadScene("MainMenu");

    }

    public void Restart()
    {
        SceneManager.LoadScene("InGameJet");
    }
}
