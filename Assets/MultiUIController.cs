using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiUIController : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    [SerializeField] TMP_Text deadText;
    [SerializeField] GameObject quitButton;

    public void PlayerDead()
    {
        gameOver.SetActive(true);
        deadText.enabled = true;
        quitButton.SetActive(false);
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        deadText.enabled = false;
        quitButton.SetActive(true);
    }
}
