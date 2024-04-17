using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_Text scoreText;
    public Board tetrisBoard;
    public Piece pieceComponent;
    public GameObject StartWordUI;
    public GameObject GameOverUI;
    public GameObject GameMenuUI;
    public GameObject TipsUI;
    [HideInInspector]
    public bool isGameStarted;

    public void Start()
    {
        isGameStarted = false;
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Button");
        //Debug.Log("UI controler: started");
    }

    public void ClearRowTrigger()
    {
        if (!tetrisBoard.IsGameOver)
        {
            scoreText.SetText((tetrisBoard.Score * 100).ToString());
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Clear Line");
        }
    }

    public void GameOverTrigger()
    {
        EndGame();
        this.GameOverUI.SetActive(true);
    }


    public void ToggleMenu()
    {
        Time.timeScale = 1 - Time.timeScale;
        this.GameMenuUI.SetActive(!this.GameMenuUI.activeSelf);
    }

    public void ToggleTips()
    {
        Time.timeScale = 1 - Time.timeScale;
        this.TipsUI.SetActive(!this.TipsUI.activeSelf);
    }

    public void ReLoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the scene to restart the game
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void StartGame()
    {
        if (!this.isGameStarted)
        {
            Time.timeScale = 1;
            this.StartWordUI.SetActive(false);
            this.isGameStarted = true;
            this.pieceComponent.OnGameStart();
            this.tetrisBoard.OnGameStart();
        }
    }


    public int EndGame()
    {
        Time.timeScale = 0;
        TMP_Text score = GameOverUI.transform.GetChild(0).GetComponent<TMP_Text>();
        score.SetText("Your score is\n" + (tetrisBoard.Score * 100).ToString());
        return tetrisBoard.Score*100;
    }
}
