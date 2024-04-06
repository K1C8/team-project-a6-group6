// Inspired by tutorial: https://www.youtube.com/watch?v=ODLzYI4d-J8&t=1864s&ab_channel=Zigurous

using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z
}

[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] cells { get; private set; }

    public void Initiate()
    {
        this.cells = Data.Cells[this.tetromino];
    }
}