using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private void Start() 
    { 
        RandomizePosition();
    }
    public void RandomizePosition()
    {
        Bounds bounds = this.gridArea.bounds;
       

        Vector2 spawnPosition = new Vector2Int(Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x)), Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y)));

        //Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
        this.transform.position = spawnPosition;
        Debug.Log(spawnPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if(other.tag == "Player")
        {   
            // Could add SFX here

            RandomizePosition();
        }
    }
}
