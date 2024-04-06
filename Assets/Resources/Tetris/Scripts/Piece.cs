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
    public int rotationIndex { get; private set; } // (0,1,2,3) storig 4 rotations
    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;


        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i=0; i < data.cells.Length; i++)
        { 
            this.cells[i] = ((Vector3Int)data.cells[i]);
        }
        //GetComponent<Board>(); 
    }

    public void Update()
    {
        this.board.Clear(this);

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
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(-1);
        }
        this.board.Set(this);
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
    }

    private void Rotate(int dir)
    {
        this.rotationIndex = (this.rotationIndex + dir + 4) % 4;

        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;

            switch (this.data.tetromino)
            {
                case Tetromino.I:
                    cell.x -= 1;
                    x = Mathf.RoundToInt(cell.x * Data.RotationMatrix[0] * dir + cell.y * Data.RotationMatrix[1] * dir);
                    y = Mathf.RoundToInt(cell.x * Data.RotationMatrix[2] * dir + cell.y * Data.RotationMatrix[3] * dir);
                    break;
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.RoundToInt(cell.x * Data.RotationMatrix[0] * dir + cell.y * Data.RotationMatrix[1] * dir);
                    y = Mathf.RoundToInt(cell.x * Data.RotationMatrix[2] * dir + cell.y * Data.RotationMatrix[3] * dir);
                    break;
                default:
                    x = Mathf.RoundToInt(cell.x * Data.RotationMatrix[0] * dir + cell.y * Data.RotationMatrix[1] * dir);
                    y = Mathf.RoundToInt(cell.x * Data.RotationMatrix[2] * dir + cell.y * Data.RotationMatrix[3] * dir);
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
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
        }

        return valid;
    }
}
