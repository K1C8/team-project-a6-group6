using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    private float _spawnMaximumInterval = 4.0f;
    private bool _isPlayerAlive = true;

    [SerializeField]
    private GameObject _enemyEntry;
    [SerializeField]
    private GameObject _containerTypeEnemy; 



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnRoutine()
    {
        while (_isPlayerAlive)
        {
            Vector3 positionToSpawn = new Vector3(Random.Range(-2f, 2f), 4, 0);
            GameObject newEnemyEntry = Instantiate(_enemyEntry, positionToSpawn, Quaternion.identity);
            newEnemyEntry.transform.parent = _containerTypeEnemy.transform;
            yield return new WaitForSeconds(Random.Range(0.5f, _spawnMaximumInterval));
        }
    }

    public void OnPlayerDeath()
    {
        _isPlayerAlive = false;
        Debug.Log("Stopping spawning enemy after player is dead.");
    }
}
