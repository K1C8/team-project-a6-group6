using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLogic : MonoBehaviour
{
    private JetSpawnManager _jetSpawnManager;
    private bool _canRespawn = true;
    private float _respawnTime;

    [SerializeField] private float _scrollingSpeed = 1f;
    [SerializeField] private int _scrollInterval = 14;
    // Start is called before the first frame update
    void Start()
    {
        _respawnTime = _scrollInterval / _scrollingSpeed;
        Debug.Log(string.Format("Respawn time interval for background is {0}", _respawnTime));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _scrollingSpeed * Time.deltaTime);
        if (transform.position.y < -3.6f && _canRespawn)
        {
            _jetSpawnManager.SpawnBackgroundNeeded();
            _canRespawn = false;
            StartCoroutine(RespawnTimer());
        }
        else if (transform.position.y < -20f)
        {
            _jetSpawnManager.BackgroundScrolledOut();
            Destroy(gameObject);
        }
    }

    public void RegisterCallback(JetSpawnManager spawnManager) {
        if (spawnManager == null)
        {
            Debug.LogError("SpawnManager instance being registered in BackgroundLogic is null.");
        }
        _jetSpawnManager = spawnManager;
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(_respawnTime);
        _canRespawn = true;
    }

}
