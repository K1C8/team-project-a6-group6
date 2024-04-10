// Inspired by tutorial: https://www.youtube.com/watch?v=ODLzYI4d-J8&t=1864s&ab_channel=Zigurous

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; } // (0,1,2,3) storing 4 rotations

    public float stepDelay;
    public float lockDelay = .5f;

    private float stepTime;
    private float lockTime; // time for locking after the tetris touches the ground?
    private bool isLocked;

    public void Initialize(Board board, Vector3Int position, TetrominoData data, float stepDelay)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay; // trigger a stepping each stepDelay amount of time
        this.lockTime = 0f;
        this.isLocked = false;
        this.stepDelay = stepDelay;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i=0; i < data.cells.Length; i++)
        { 
            this.cells[i] = ((Vector3Int)data.cells[i]);
        }
    }

    public void Update()
    {
        if (this.isLocked) { return; }

        this.board.ClearTile(this);
        this.lockTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Rotate();
        }
        this.board.Set(this);
        
        if (Time.time >= this.stepTime)
        {
            Step();
        }
    
    }

    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;
        this.board.ClearTile(this);
        bool valid = Move(Vector2Int.down);

        if (valid)
        {
            this.lockTime = 0f;
        }

        if (this.lockTime >= this.lockDelay)
        {
            //Debug.Log("[Lock] in [Step]. lockTime is " + this.lockTime +
            //", valid flag is " + valid + " ---------- " + Time.time.ToString());
            Lock();
        }
        this.board.Set(this);
    }

    private void Lock()
    {
        this.board.Set(this);
        this.isLocked = true;

        if (!this.board.isGameOver)
        {
            this.lockTime = 0f;
            this.board.ClearRows();
            this.board.GeneratePiece();
        }
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
        Lock();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsPositionValid(this, newPosition);

        if (valid)
        {
            this.position = newPosition;
            //this.lockTime = 0f;
        }

        return valid;
    }

    private void Rotate()
    {
        int newRotationIndex = (this.rotationIndex + 1) % 4;

        CoordinateRotate(1);

        if (!TestWallKicks(this.rotationIndex))
        {
            CoordinateRotate(-1);
            return;
        }

        this.rotationIndex = newRotationIndex;
        return;
    }

    private void CoordinateRotate(int direction)
    {
        if (direction > 0)
        {
            for (int i = 0; i < this.cells.Length; i++)
            {
                Vector3 cell = this.cells[i];

                int x, y;

                switch (this.data.tetromino)
                {
                    case Tetromino.I:
                        //if ()
                        cell.x -= 1f;
                        x = Mathf.RoundToInt(cell.y);
                        y = Mathf.RoundToInt(-cell.x);
                        break;
                    case Tetromino.O:
                        x = Mathf.RoundToInt(cell.x);
                        y = Mathf.RoundToInt(cell.y);
                        break;
                    default:
                        x = Mathf.RoundToInt(cell.y);
                        y = Mathf.RoundToInt(-cell.x);
                        break;
                }

                this.cells[i] = new Vector3Int(x, y, 0);
            }
        }
        else
        {
            for (int i = 0; i < this.cells.Length; i++)
            {
                Vector3 cell = this.cells[i];

                int x, y;

                switch (this.data.tetromino)
                {
                    case Tetromino.I:
                        cell.y += 1f;
                        x = Mathf.RoundToInt(-cell.y);
                        y = Mathf.RoundToInt(cell.x);
                        break;
                    case Tetromino.O:
                        x = Mathf.RoundToInt(cell.x);
                        y = Mathf.RoundToInt(cell.y);
                        break;
                    default:
                        x = Mathf.RoundToInt(-cell.y);
                        y = Mathf.RoundToInt(cell.x);
                        break;
                }

                this.cells[i] = new Vector3Int(x, y, 0);
            }
        }
    }

    // // to ensure that a rotation or translation is valid before rotate it.
    private bool TestWallKicks(int rotationIndex)
    {
        int wallKickIndex = (2 * rotationIndex + 8) % 8;

        Vector2Int tranlation = new Vector2Int();

        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            tranlation = this.data.wallKicks[wallKickIndex, i];
            
            if (Move(tranlation))
            {
                return true;
            }
        }

        return false;
    }
}
