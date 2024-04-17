using System.Collections.Generic;
using UnityEngine;
using Utils;
using System;
using TMPro;

public class Snake : MonoBehaviour
{

    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private int snakeBodySize;
    private int score;
    Boolean isMovingUp = false;
    Boolean isMovingDown = false;
    Boolean isMovingRight = false;
    Boolean isMovingLeft = false;
    private List<Vector2Int> snakeMovePositionList;
    public Sprite segmentSprite;

    public TextMeshProUGUI scoreText;
    public GameObject gameOver;

    public Boolean isGameOver;
    public SnakeInGameUIController uiController;

    public Sprite upHeadSprite;
    public Sprite downHeadSprite;
    public Sprite leftHeadSprite;
    public Sprite rightHeadSprite;

    private SpriteRenderer headSpriteRenderer;
    [SerializeField] MultiUIController multiUIController;
    [SerializeField] SnakeMultiSimulator snakeMultiSimulator;


    private void Awake()
    {   

        gridPosition = new Vector2Int(0, 0);
        gridMoveTimerMax = .3f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1, 0);

        snakeMovePositionList = new List<Vector2Int>();
        snakeBodySize = 3;
        score = 0;
        Time.timeScale = 0;
        isGameOver = false;

        headSpriteRenderer = GetComponent<SpriteRenderer>();
        headSpriteRenderer.sprite = rightHeadSprite; // Default facing right
    }


    private void Update()
    {
        if (!isGameOver && uiController.isRunning)
        {
            HandleInput();
            HandleGridMovement();
            if (!MultiSingleManager.Instance.isMulti)
            {
                scoreText.text = score.ToString();
            }
 
        }  
    }

    private void HandleInput()
    {
        if (isMovingUp)
        {
            if (gridMoveDirection.y != -1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = +1;
                headSpriteRenderer.sprite = upHeadSprite;
            }
        }
        if (isMovingDown)
        {
            if (gridMoveDirection.y != +1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;
                headSpriteRenderer.sprite = downHeadSprite;
            }
        }
        if (isMovingLeft)
        {
            if (gridMoveDirection.x != +1)
            {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
                headSpriteRenderer.sprite = leftHeadSprite;
            }
        }
        if (isMovingRight)
        {
            if (gridMoveDirection.x != -1)
            {
                gridMoveDirection.x = +1;
                gridMoveDirection.y = 0;
                headSpriteRenderer.sprite = rightHeadSprite;
            }
        }
    }

    // Read the inputs from the game control panel
    public void MoveUp()
    {
        isMovingUp = true;
        isMovingDown = false;
        isMovingRight = false;
        isMovingLeft = false;
        
    }
    public void MoveDown()
    {
        isMovingUp = false;
        isMovingDown = true;
        isMovingRight = false;
        isMovingLeft = false;
        
    }

    public void MoveLeft()
    {
        isMovingUp = false;
        isMovingDown = false;
        isMovingRight = false;
        isMovingLeft = true;
        
    }

    public void MoveRight()
    {
        isMovingUp = false;
        isMovingDown = false;
        isMovingRight = true;
        isMovingLeft = false;
        
    }



    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            snakeMovePositionList.Insert(0, gridPosition);

            gridPosition += gridMoveDirection;

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            for (int i = 0; i < snakeMovePositionList.Count; i++)
            {
                Vector2Int snakeMovePosition = snakeMovePositionList[i];
                World_Sprite worldSprite = World_Sprite.Create(new Vector3(snakeMovePosition.x, snakeMovePosition.y), Vector3.one * .75f, segmentSprite, Color.white);
                worldSprite.AddBoxCollider2D(); 
                worldSprite.SetTag("Obstacle");
                FunctionTimer.Create(worldSprite.DestroySelf, gridMoveTimerMax);
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            
        }
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            AudioManager.Instance.PlaySFX("Eat");
            snakeBodySize++;
            score++;
            if (MultiSingleManager.Instance.isMulti)
            {
                snakeMultiSimulator.UpdateScore("YOU", 100);
            }
        }
        else if (other.gameObject.CompareTag("Obstacle")) 
        {
            AudioManager.Instance.PlaySFX("Lose");
            if (MultiSingleManager.Instance.isMulti)
            {
                MultiGameOver();
            }
            else
            {
                GameOver();
            }
        } 
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
        isGameOver = true;
    }

    public void MultiGameOver()
    {
        multiUIController.PlayerDead();
        isGameOver = true;
        snakeMovePositionList.Clear();
        Destroy(gameObject);
    }

    // Return the full list of positions occupied by the snake: Head + Body
    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }
    public bool Occupies(int x, int y)
    {
        foreach (Vector2Int position in snakeMovePositionList)
        {
            if (position.x == x &&
                position.y == y)
            {
                return true;
            }
        }

        return false;
    }
}
