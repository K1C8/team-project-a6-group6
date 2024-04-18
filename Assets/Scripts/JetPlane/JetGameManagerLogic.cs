using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JetGameManagerLogic : MonoBehaviour 
{
    private int _playerScore;
    private int _playerHitPoint;
    private int _playerLives;
    private string _multiManagerName = "MultiSingleManager";
    private string _playerHpLivesFormat = "Lives: {0}\nHit Points: {1}";
    private bool _isMultiMode = false;

    [SerializeField] private GameObject _gameOverSingleMode;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private Slider _effectVolumeSlider;
    [SerializeField] private TMP_Text _hitPointsAndLivesText;
    [SerializeField] private TMP_Text _singleScoreText;
    [SerializeField] private TMP_Text _singleNameText;
    [SerializeField] private JetMultiSimulator jetMultiSimulator;
    [SerializeField] private MultiUIController multiUIController;

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

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }

        GameObject multiManager = GameObject.Find(_multiManagerName);
        if (multiManager != null)
        {
            _isMultiMode = true;
        } else
        {
            if (_singleNameText != null)
            {
                _singleNameText.text = "You";
            } else
            {
                Debug.LogError("Cannot find the instance of _singleNameText.");
            }
            Debug.Log("The JetGameManagerLogic now operates in single player mode.");
        }

        if (_singleScoreText == null && !_isMultiMode)
        {
            Debug.LogError("Cannot find the instance of _singleScoreText.");
        }

        if (_hitPointsAndLivesText != null)
        {
            _hitPointsAndLivesText.text = string.Format(_playerHpLivesFormat, 2, 100);
        }


        Time.timeScale = 0f;

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

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Button");
        }
    }

    public void InitScore()
    {
        if (!_isMultiMode)
        {
            _singleScoreText.text = "0";
        }
    }

    public void UpdateSingleScore()
    {
        if (MultiSingleManager.Instance.isMulti)
        {
            jetMultiSimulator.UpdateScore("YOU", _playerScore);
        }
        else
        {
            string _scoreText = _playerScore.ToString();
            _singleScoreText.text = _scoreText;
        }
    }


    public void Pause()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isMultiMode)
        {
            UpdateSingleScore();
        }

        UpdatePlayerHitPointAndLives();
    }


    private void UpdatePlayerHitPointAndLives()
    {
        _hitPointsAndLivesText.text = string.Format(_playerHpLivesFormat, PlayerLives, PlayerHitPoint);
    }

    public int PlayerScore
    {
        get { return _playerScore; }
        set { _playerScore = value; }
    }

    public int PlayerHitPoint
    {
        get { return _playerHitPoint; }
        set 
        {
            if (_playerLives > 0) 
            {
                _playerHitPoint = value;
            } else
            {
                _playerHitPoint = 0;
            }
             
        }
    }

    public int PlayerLives
    {
        get { return _playerLives; }
        set { _playerLives = value; }
    }

    // Member to call when user touches a button
    public void PressToStart()
    {

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic("BGM For Jet");
        }
        JetPlayerController JetPlayer = (FindObjectsByType<JetPlayerController>(FindObjectsInactive.Include, FindObjectsSortMode.None))[0];
        JetSpawnManager EnemySpawnManager = (FindObjectsByType<JetSpawnManager>(FindObjectsInactive.Include, FindObjectsSortMode.None))[0];
        GameObject pressToStartText = GameObject.Find("PressToStart");
        if (JetPlayer != null && EnemySpawnManager != null && pressToStartText != null)
        {
            JetPlayer.gameObject.SetActive(true);
            EnemySpawnManager.gameObject.SetActive(true);
            pressToStartText.SetActive(false);
        }

        if (MultiSingleManager.Instance.isMulti)
        {
            JetPlayer.gameObject.SetActive(true);
            EnemySpawnManager.gameObject.SetActive(true);
        }
        Time.timeScale = 1.0f;
    }

    IEnumerator GameOverProcess()
    {
        yield return new WaitForSeconds(3f);
        _gameOverSingleMode.SetActive(true);
    }

    public void OnGameOver()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }
        if (MultiSingleManager.Instance.isMulti)
        {
            multiUIController.PlayerDead();
            jetMultiSimulator.setButtonToDisable(true);
        }
        else
        {
            StartCoroutine(GameOverProcess());
        }
    }

    public void QuitGame()
    {

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic("BGM");
        }
        SceneManager.LoadScene("MainMenu");

    }

    public void Restart()
    {
        SceneManager.LoadScene("Jet");
    }

}
