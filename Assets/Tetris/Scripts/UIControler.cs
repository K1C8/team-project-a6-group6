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
    public GameObject GameOverUI;
    public GameObject StartWordUI;
    [HideInInspector]
    public bool isGameStarted;

    public void ClearRowTrigger()
    {
        if (!tetrisBoard.IsGameOver)
        {
            scoreText.text = "Current Score: " + tetrisBoard.Score.ToString();
            AudioManager.Instance.PlaySFX("Clear Line");
        }
    }

    public void GameOverTrigger()
    {
        this.GameOverUI.SetActive(true);
    }

    public void StartGame()
    {
        if (!this.isGameStarted)
        {
            this.StartWordUI.SetActive(false);
            this.isGameStarted = true;
            this.pieceComponent.isGameStarted = true;
            AudioManager.Instance.PlaySFX("Button");
            this.tetrisBoard.GeneratePiece();
        }
    }


    public void ReLoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the scene to restart the game
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
