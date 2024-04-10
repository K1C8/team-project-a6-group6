// Inspired by tutorial: https://www.youtube.com/watch?v=ODLzYI4d-J8&t=1864s&ab_channel=Zigurous

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public TetrominoData[] tetrominoes;
    public Vector3Int generatePosition;
    public Piece activePiece { get; private set; }
    public Tilemap tilemap { get; private set; }
    public Vector2Int boardSize = new Vector2Int(10,14);
    public int score;
    public bool isGameOver;

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize + new Vector2Int(0, 4));
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        this.score = 0;
        this.isGameOver = false;

        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initiate();
        }
    }

    // Update is called once per frame
    private void Start()
    {
        GeneratePiece();
    }

    public void GeneratePiece()
    {
        int randomCellIndex = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[randomCellIndex];
        float stepSpeed = this.score >= Data.ScoreVersusSpeed.Length ? 0.4f : Data.ScoreVersusSpeed[this.score].y;
        this.activePiece.Initialize(this, generatePosition, data, stepSpeed);
        if (this.tilemap.HasTile(generatePosition))
        {
            this.isGameOver = true;
        }
        Set(this.activePiece);
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void ClearTile(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public void ClearRows()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        float tmp = Time.time;
        while (row < bounds.yMax)
        {
            if (Time.time - tmp > 2f) break;
            if (IsLineFull(row))
            {
                ClearSingleRow(row);
                //Debug.Log("ClearRows | This line is full!");
                //break;
            }
            else
            {
                row++;
            }
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new(col, row, 0);
            if (!this.tilemap.HasTile(position))
            {
                //Debug.Log("It has tiles at position :" + position);
                return false;
            }
        }
        return true;
    }

    private void ClearSingleRow(int row)
    {
        this.score += 1;
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new(col, row, 0);
            this.tilemap.SetTile(position, null);    
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);
                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }
            row++;
        }
    }

    public bool IsPositionValid(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        // check the four pieces, if out of bound or already take place, then false.

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;
            if (!bounds.Contains((Vector2Int)tilePosition)) 
            {
                //Debug.Log("out of bound!");
                return false; 
            }
            if (this.tilemap.HasTile(tilePosition)) 
            { 
                //Debug.Log("has tile!");
                return false;
            }
        }
        return true;
    }
}
