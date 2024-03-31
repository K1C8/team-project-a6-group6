using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    // Initailly make the sanke move to right
    private Vector2 direction = Vector2.right;

    public void MoveUp()
    {
        direction = Vector2.up;
    }

    public void MoveDown()
    {
        direction = Vector2.down;
    }

    public void MoveLeft()
    {
        direction = Vector2.left;
    }

    public void MoveRight()
    {
        direction = Vector2.right;
    }

    private void FixedUpdate()
    {
        int x = Mathf.RoundToInt(transform.position.x + direction.x);
        int y = Mathf.RoundToInt(transform.position.y + direction.y);
        this.transform.position = new Vector2(x, y);
    }
}
