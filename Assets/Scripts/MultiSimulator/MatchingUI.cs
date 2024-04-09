using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchingUI : MonoBehaviour
{
    [SerializeField] GameObject loadingBar;
    [SerializeField] TMP_Text loadingText;
    public float timeMatching = 3f;
    [SerializeField] GameObject gameBoard;
    [SerializeField] TMP_Text gameName;
    [SerializeField] GameObject gameImage;
    [SerializeField] private GameScenes[] gameScenes;
    void Start()
    {
        StopAllCoroutines();
        AudioManager.Instance.PlayMusic("BGM For Matching");
        StartCoroutine(SimulateMatching());
    }

    // Coroutine to simulate matching progress
    private IEnumerator SimulateMatching()
    {
        // Wait for timeMatching seconds
        yield return new WaitForSeconds(timeMatching);

        // Display "Found!"
        loadingText.text = "Found!";

        // Wait for 1 second
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.StopMusic();
        // Activate loading bar
        loadingBar.SetActive(false);

        // Select a random game
        int randomIndex = Random.Range(0, gameScenes.Length);
        GameScenes selectedGame = gameScenes[randomIndex];
        AudioManager.Instance.PlaySFX("Matched");
        // Activate game UI after 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        gameName.text = selectedGame.sceneName;
        gameImage.GetComponent<Image>().sprite = selectedGame.gameIcon;
        gameBoard.SetActive(true);

        yield return new WaitForSeconds(1f);
        // Open corresponding scene by sceneName
        MultiSingleManager.Instance.isMulti = true;
        SceneManager.LoadScene(selectedGame.sceneName);

    }
}

[System.Serializable]
public class GameScenes
{
    public string sceneName;
    public Sprite gameIcon;
}