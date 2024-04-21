// Inspired by tutorial: https://medium.com/@dhunterthornton

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JetSpawnManager : MonoBehaviour
{
    private bool _isPlayerAlive = true;
    private bool _needToRespawnBackground = false;
    private float _spawnMaximumInterval = 2.5f;
    private float _spawnMinimumInterval = 0.25f;
    private float _nextSpawnYOffset = 11.72f;
    private float _midBossInterval = 0f;
    private GameObject _firstSpawnedBackground;
    private GameObject _secondSpawnedBackground;

    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _enemyEntry;
    [SerializeField] private GameObject _enemyMidBoss;
    [SerializeField]
    private GameObject _containerTypeEnemy;
    [SerializeField] float _ratioEnemySpawnSpeedUp = 0.996f;
    [SerializeField] float _bossInterval = 70f;



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
            float next = Random.Range(_spawnMinimumInterval, _spawnMaximumInterval);
            if (_spawnMinimumInterval > 0.1f) 
            { 
                _spawnMinimumInterval *= _ratioEnemySpawnSpeedUp;
            }
            if (_spawnMaximumInterval > 1f)
            { 
                _spawnMaximumInterval *= _ratioEnemySpawnSpeedUp;
            }
            _midBossInterval += next;
            if (_midBossInterval > _bossInterval && _enemyMidBoss != null)
            {
                Vector3 positionToSpawnMidBoss = new Vector3(Random.Range(-0.5f, 0.5f), 3, 0);
                GameObject newEnemyMidBoss = Instantiate(_enemyMidBoss, positionToSpawnMidBoss, Quaternion.identity);
                newEnemyMidBoss.transform.parent = _containerTypeEnemy.transform;
                _midBossInterval = 0;
            }
            yield return new WaitForSeconds(next);
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
            _secondSpawnedBackground = Instantiate(_background, new Vector3(0, _nextSpawnYOffset, 0), Quaternion.identity);
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
