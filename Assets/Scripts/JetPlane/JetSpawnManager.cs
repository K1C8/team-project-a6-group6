using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JetSpawnManager : MonoBehaviour
{
    private bool _isPlayerAlive = true;
    private bool _needToRespawnBackground = false;
    private float _spawnMaximumInterval = 4.0f;
    private GameObject _firstSpawnedBackground;
    private GameObject _secondSpawnedBackground;

    [SerializeField] private GameObject _background;
    [SerializeField]
    private GameObject _enemyEntry;
    [SerializeField]
    private GameObject _containerTypeEnemy; 



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
        _firstSpawnedBackground = Instantiate(_background, new Vector3(0f, 4.4f, 0f), Quaternion.identity);
        _firstSpawnedBackground.GetComponent<BackgroundLogic>().RegisterCallback(this);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBackground();
    }

    IEnumerator EnemySpawnRoutine()
    {
        while (_isPlayerAlive)
        {
            Vector3 positionToSpawn = new Vector3(Random.Range(-2f, 2f), 3, 0);
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


    private void UpdateBackground()
    {
        RespawnBackground();
    }

    public void RespawnBackground()
    {
        if (_needToRespawnBackground)
        {
            _secondSpawnedBackground = Instantiate(_background, new Vector3(0, 11.75f, 0), Quaternion.identity);
            _secondSpawnedBackground.GetComponent <BackgroundLogic>().RegisterCallback(this);
            _needToRespawnBackground = false;
        }
    }

    internal void SpawnBackgroundNeeded()
    {
        _needToRespawnBackground = true;
    }

    public void BackgroundScrolledOut()
    {
        _firstSpawnedBackground = _secondSpawnedBackground;
        _secondSpawnedBackground = null;
    }
}
