using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SnakeMultiSimulator : MonoBehaviour
{
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] GameObject player3;
    [SerializeField] GameObject player4;
    [SerializeField] GameObject UIcontroller;
    [SerializeField] Snake snake;
    [SerializeField] GameObject multiUI;
    [SerializeField] GameObject[] objectsToHide;
    [SerializeField] Button[] buttonsToDisable;
    [SerializeField] TMP_Text gameStartCD;
    [SerializeField] TMP_Text inGameCD;
    public float gameDuration = 180f; // 3 minutes in seconds
    public bool isGameOver = false;
    public void Start()
    {
        player1.GetComponentInChildren<TMP_Text>().text = "YOU";
        if (MultiSingleManager.Instance.isMulti)
        {
            player1.SetActive(true);
            player2.SetActive(true);
            player3.SetActive(true);
            player4.SetActive(true);
            setButtonToDisable(true);
            setObjectsToHide(true);
            StartCoroutine(StartGameCountdown());
            Time.timeScale = 0;
        }
        else
        {
            player1.SetActive(true);
            player2.SetActive(false);
            player3.SetActive(false);
            player4.SetActive(false);
            multiUI.SetActive(false);
        }
    }


    private void setObjectsToHide(bool makeHide)
    {
        if (makeHide)
        {
            foreach (GameObject o in objectsToHide)
            {
                o.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject o in objectsToHide)
            {
                o.SetActive(true);
            }
        }
    }
    private void setButtonToDisable(bool makeDisable)
    {
        if (makeDisable)
        {
            foreach (Button button in buttonsToDisable)
            {
                button.enabled = false;
            }
        }
        else
        {
            foreach (Button button in buttonsToDisable)
            {
                button.enabled = true;
            }
        }
    }

    private void Update()
    {
        if (!isGameOver && Time.timeScale == 1)
        {
            gameDuration -= Time.deltaTime;
            if (gameDuration <= 0)
            {
                GameOver();
            }
            else
            {
                UpdateInGameCountdown();
            }
        }
    }

    private IEnumerator StartGameCountdown()
    {
        gameStartCD.text = "Three";
        yield return new WaitForSecondsRealtime(1f);

        gameStartCD.text = "Two";
        yield return new WaitForSecondsRealtime(1f);

        gameStartCD.text = "One";
        yield return new WaitForSecondsRealtime(1f);

        gameStartCD.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);

        gameStartCD.gameObject.SetActive(false); // Hide the countdown text after "GO!"
        Time.timeScale = 1; // Resume game time
        setButtonToDisable(false); // Enable buttons

        // Start in-game countdown
        StartCoroutine(InGameCountdown());
    }

    private IEnumerator InGameCountdown()
    {
        while (gameDuration > 0)
        {
            UpdateInGameCountdown();
            yield return null;
        }

    }

    private void UpdateInGameCountdown()
    {
        int minutes = Mathf.FloorToInt(gameDuration / 60);
        int seconds = Mathf.FloorToInt(gameDuration % 60);
        inGameCD.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void GameOver()
    {
        isGameOver = true;
        inGameCD.enabled = false;
        multiUI.GetComponent<MultiUIController>().GameOver();
    }
}
