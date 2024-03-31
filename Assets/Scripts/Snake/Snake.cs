using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Windows;

public class Snake : MonoBehaviour
{
    // Initailly make the sanke move to right
    private Vector2Int direction = Vector2Int.right;
    private List<Transform> segments = new List<Transform>();
    public Transform segmentPrefab;
    public GameObject gameView;
    public GameObject gameOver;
    public int initialSize = 4;
    private int score = 0;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        ResetState();
        scoreText.text = "Score: 0";
    }
   
    private void FixedUpdate()
    {
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        int x = Mathf.RoundToInt(transform.position.x + direction.x);
        int y = Mathf.RoundToInt(transform.position.y + direction.y);
        this.transform.position = new Vector2(x, y);

        scoreText.text = "Score: " + score.ToString();
    }

    // Read the inputs from the game control panel
    public void MoveUp()
    {
        direction = Vector2Int.up;
    }
    public void MoveDown()
    {
        direction = Vector2Int.down;
    }

    public void MoveLeft()
    {
        direction = Vector2Int.left;
    }

    public void MoveRight()
    {
        direction = Vector2Int.right;
    }

    private void Grow()
    {
        // Instantiate a new UI segment
        Transform segment = Instantiate(segmentPrefab, gameView.transform);
        segment.position = segments[segments.Count - 1].position;
        // Add the new segment to the list
        segments.Add(segment);
        
    }

    public void ResetState()
    {
        direction = Vector2Int.right;
        this.transform.position = gameView.transform.position;

        // Start at 1 to skip destroying the head
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        // Clear the list but add back this as the head
        segments.Clear();
        segments.Add(transform);
        score = 0;

        // -1 since the head is already in the list
        for (int i = 0; i < initialSize - 1; i++)
        {
            Grow();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Grow();
            score++;
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
            ResetState();
        }
    }

    private void GameOver() 
    { 
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume() 
    {
        Time.timeScale = 1;
    }
}
