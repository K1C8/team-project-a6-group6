using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class MultiUIController : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    [SerializeField] TMP_Text deadText;
    [SerializeField] GameObject quitButton;
    [SerializeField] GameObject rankBoard;
    [SerializeField] GameObject first;
    [SerializeField] GameObject second;
    [SerializeField] GameObject third;

    public void PlayerDead()
    {
        gameOver.SetActive(true);
        deadText.enabled = true;
        quitButton.SetActive(false);
    }

    public void GameOver(Dictionary<string, int> playerScores)
    {
        gameOver.SetActive(true);
        deadText.enabled = false;
        quitButton.SetActive(true);
        rankBoard.SetActive(true);
        var sortedPlayers = playerScores.OrderByDescending(x => x.Value);
        int rank = 1;
        foreach (var player in sortedPlayers)
        {
            if (rank < 4)
            {
                GameObject playerUI = GetPlayerUI("Player" + rank.ToString());
                Transform playerNameUI = playerUI.transform.Find("Name");
                Transform playerScoreUI = playerUI.transform.Find("Score");
                playerNameUI.GetComponent<TMP_Text>().text = player.Key;
                playerScoreUI.GetComponentInChildren<TMP_Text>().text = player.Value.ToString();
            }
            rank++;
        }
    }
    private GameObject GetPlayerUI(string playerName)
    {
        switch (playerName)
        {
            case "Player1":
                return first;
            case "Player2":
                return second;
            case "Player3":
                return third;
            default:
                return null;
        }
    }
}
