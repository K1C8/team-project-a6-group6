using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Food : MonoBehaviour
{
    public Transform topWall; // Assign the top wall of the blue box in the inspector.
    public Transform bottomWall; // Assign the bottom wall of the blue box in the inspector.
    public Transform leftWall; // Assign the left wall of the blue box in the inspector.
    public Transform rightWall; // Assign the right wall of the blue box in the inspector.


    private void Start() 
    { 
        RandomizePosition();
    }
    public void RandomizePosition()
    {
        // Get the thickness of each wall based on its collider size and local scale
        float topWallThickness = topWall.GetComponent<BoxCollider2D>().size.y * topWall.localScale.y;
        float bottomWallThickness = bottomWall.GetComponent<BoxCollider2D>().size.y * bottomWall.localScale.y;
        float leftWallThickness = leftWall.GetComponent<BoxCollider2D>().size.x * leftWall.localScale.x;
        float rightWallThickness = rightWall.GetComponent<BoxCollider2D>().size.x * rightWall.localScale.x;

        // Calculate the inner spawn area
        float minX = leftWall.position.x + leftWallThickness / 2;
        float maxX = rightWall.position.x - rightWallThickness / 2;
        float minY = bottomWall.position.y + bottomWallThickness / 2;
        float maxY = topWall.position.y - topWallThickness / 2;

        Vector2 spawnPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        //Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
        this.transform.position = spawnPosition;
        Debug.Log(spawnPosition);
    }
}
