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
}
