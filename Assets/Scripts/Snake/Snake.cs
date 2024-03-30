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
        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + direction.x,
            Mathf.Round(this.transform.position.y) + direction.y,
            0.0f
        );
    }
}
