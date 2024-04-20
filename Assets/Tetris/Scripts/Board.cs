// Inspired by tutorial: https://www.youtube.com/watch?v=ODLzYI4d-J8&t=1864s&ab_channel=Zigurous

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public TetrisUIController controller;
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominoes;
    public SpriteRenderer spriteR;
    public Piece activePiece { get; private set; }
    public RectInt Bounds { get; private set; }
    private Vector3Int generatePosition;
    private Vector3Int nextTilePosition1;
    private Vector3Int nextTilePosition2;
    private Vector2Int nextTilesList;
    public Vector2Int boardSize;
    public int Score { get; private set; }
    public bool IsGameOver { get; private set; }
    [SerializeField] MultiUIController multiUIController;
    [SerializeField] TetrisMultiSimulator tetrisMultiSimulator;

    // Start is called before the first frame update
    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        this.boardSize = Vector2Int.FloorToInt(spriteR.size);
        Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
        this.Bounds = new RectInt(position, this.boardSize + new Vector2Int(0, 4));

        this.generatePosition = new Vector3Int(-1, this.boardSize.y / 2 - 1, 0);
        this.nextTilePosition1 = new Vector3Int((int)spriteR.size.x - 3, -1, 0);
        this.nextTilePosition2 = new Vector3Int((int)spriteR.size.x - 3, -6, 0);

        this.Score = 0;
        this.IsGameOver = false;

        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initiate();
        }
    }

    private void Start()
    {
        this.IsGameOver = false;
        //Debug.Log("Board: started");
    }

    public void OnGameStart()
    {
        this.nextTilesList = new Vector2Int(Random.Range(0, this.tetrominoes.Length), Random.Range(0, this.tetrominoes.Length));
        GeneratePiece();
    }

    public void GeneratePiece()
    {
        ClearTilePreview();

        int generatedCellIndex = this.nextTilesList.x;
        this.nextTilesList = new Vector2Int(this.nextTilesList.y, Random.Range(0, this.tetrominoes.Length));

        SetTilePreview();

        if (this.tilemap.HasTile(generatePosition))
        {
            this.IsGameOver = true;
        }
        else
        {
            TetrominoData data = this.tetrominoes[generatedCellIndex];
            float stepSpeed = this.Score >= Data.ScoreVersusSpeed.Length ? 0.4f : Data.ScoreVersusSpeed[this.Score].y;
            Debug.Log("stepSpeed : " + stepSpeed);
            this.activePiece.Initialize(this, generatePosition, data, stepSpeed);
            Set(this.activePiece);
        }
        
        if (this.IsGameOver)
        {
            if (MultiSingleManager.Instance.isMulti)
            {
                multiUIController.PlayerDead();
                tetrisMultiSimulator.setButtonToDisable(true);
            }
            else
            {
                this.controller.GameOverTrigger();
                Debug.Log("Triggered Gameover");
            }
        }
    }

    private void SetTilePreview()
    {
        Vector3Int[] cellsList1 = new Vector3Int[this.tetrominoes[this.nextTilesList.x].cells.Length];
        for (int i = 0; i < cellsList1.Length; i++)
        {
            cellsList1[i] = ((Vector3Int)this.tetrominoes[this.nextTilesList.x].cells[i]);
            Vector3Int tilePosition = cellsList1[i] + this.nextTilePosition1;
            this.tilemap.SetTile(tilePosition, this.tetrominoes[this.nextTilesList.x].tile);
        }

        Vector3Int[] cellsList2 = new Vector3Int[this.tetrominoes[this.nextTilesList.y].cells.Length];
        for (int i = 0; i < cellsList2.Length; i++)
        {
            cellsList2[i] = ((Vector3Int)this.tetrominoes[this.nextTilesList.y].cells[i]);
            Vector3Int tilePosition = cellsList2[i] + this.nextTilePosition2;
            this.tilemap.SetTile(tilePosition, this.tetrominoes[this.nextTilesList.y].tile);
        }
    }

    private void ClearTilePreview()
    {
        //this.tilemap.SetTile(this.nextTilePosition1, null);
        Vector3Int[] cellsList1 = new Vector3Int[this.tetrominoes[this.nextTilesList.x].cells.Length];
        for (int i = 0; i < cellsList1.Length; i++)
        {
            cellsList1[i] = ((Vector3Int)this.tetrominoes[this.nextTilesList.x].cells[i]);
            Vector3Int tilePosition = cellsList1[i] + this.nextTilePosition1;
            this.tilemap.SetTile(tilePosition, null);
        }

        //this.tilemap.SetTile(this.nextTilePosition2, null);
        Vector3Int[] cellsList2 = new Vector3Int[this.tetrominoes[this.nextTilesList.y].cells.Length];
        for (int i = 0; i < cellsList2.Length; i++)
        {
            cellsList2[i] = ((Vector3Int)this.tetrominoes[this.nextTilesList.y].cells[i]);
            Vector3Int tilePosition = cellsList2[i] + this.nextTilePosition2;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public void Set(int tileIndex)
    {
        this.tilemap.SetTile(this.nextTilePosition1, null);
        Vector3Int[] cellsList1 = new Vector3Int[this.tetrominoes[tileIndex].cells.Length];
        for (int i = 0; i < cellsList1.Length; i++)
        {
            cellsList1[i] = ((Vector3Int)this.tetrominoes[tileIndex].cells[i]);
            Vector3Int tilePosition = cellsList1[i] + this.nextTilePosition1;
            this.tilemap.SetTile(tilePosition, this.tetrominoes[tileIndex].tile);
        }
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
        this.Score += 1;
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
        if (MultiSingleManager.Instance.isMulti)
        {
            tetrisMultiSimulator.UpdateScore("YOU", 100);
        }
        else
        {
            this.controller.ClearRowTrigger();
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
