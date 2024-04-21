// Inspired by tutorial: https://medium.com/@dhunterthornton

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
    private string _multiManagerName = "MultiSingleManager";
    private string _playerHpLivesFormat = "Lives: {0}\nHP: {1}";
    private bool _isMultiMode = false;
    private bool _hasStarted = false;
    private Color _hpAndLivesTextColor = Color.white;

    [SerializeField] private GameObject _gameOverSingleMode;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private Slider _effectVolumeSlider;
    [SerializeField] private TMP_Text _hpAndLivesText;
    [SerializeField] private TMP_Text _singleScoreText;
    [SerializeField] private TMP_Text _singleNameText;
    [SerializeField] private JetMultiSimulator jetMultiSimulator;
    [SerializeField] private JetPlayerController _jetPlayerController;
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

        if (_hpAndLivesText != null)
        {
            _hpAndLivesText.text = string.Format(_playerHpLivesFormat, 2, 100);
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

        UpdatePlayerHpAndLives();
    }


    private void UpdatePlayerHpAndLives()
    {
        _hpAndLivesText.text = string.Format(_playerHpLivesFormat, PlayerLives, PlayerHp);
        _hpAndLivesText.color = _hpAndLivesTextColor;
    }

    public int PlayerScore
    {
        get { return _playerScore; }
        set { _playerScore = value; }
    }

    public int PlayerHp
    {
        get 
        {
            if (_jetPlayerController != null)
            {
                return _jetPlayerController.Hp;
            }
            return 0;
        }
    }
    public int PlayerLives
    {
        get
        {
            if (_jetPlayerController != null)
            {
                return _jetPlayerController.Lives;
            }
            return 0;
        }
    }

    public Color HpAndLivesTextColor
    {
        get { return _hpAndLivesTextColor; }
        set { _hpAndLivesTextColor = value;}
    }

    public JetPlayerController Player
    {
        get { return _jetPlayerController; }
        set { _jetPlayerController = value; }

    }


    // Member to call when user touches button B on the gamepad or started by multiplayer (simulation) mode.
    public void PressToStart()
    {
        /*
        JetPlayerController[] tmpJetPlayerArray = (FindObjectsByType<JetPlayerController>(FindObjectsInactive.Include, FindObjectsSortMode.None));
        JetSpawnManager[] tmpJetSpawnArray = (FindObjectsByType<JetSpawnManager>(FindObjectsInactive.Include, FindObjectsSortMode.None));
        if (tmpJetPlayerArray.Length > 0 && tmpJetSpawnArray.Length > 0)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusic("BGM For Jet");
            }
            JetPlayerController JetPlayer = tmpJetPlayerArray[0];
            JetSpawnManager EnemySpawnManager = tmpJetSpawnArray[0];
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
        }
        Time.timeScale = 1.0f;
        */
        if (!_hasStarted)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusic("BGM For Jet");
            }
            JetPlayerController[] tmpJetPlayerArray = (FindObjectsByType<JetPlayerController>(FindObjectsInactive.Include, FindObjectsSortMode.None));
            JetSpawnManager[] tmpJetSpawnArray = (FindObjectsByType<JetSpawnManager>(FindObjectsInactive.Include, FindObjectsSortMode.None));
            JetPlayerController JetPlayer = tmpJetPlayerArray[0];
            JetSpawnManager EnemySpawnManager = tmpJetSpawnArray[0];
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
            _hasStarted = true;
        }
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
