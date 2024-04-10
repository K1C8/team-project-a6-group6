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
        this.activePiece.Initialize(this, generatePosition, data);
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

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
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
                Debug.Log("out of bound!");
                return false; 
            }
            if (this.tilemap.HasTile(tilePosition)) 
            { 
                Debug.Log("has tile!");
                return false;
            }
        }
        return true;
    }
}
