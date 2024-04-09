using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPlayerController : MonoBehaviour
{
    private float _fireInterval = 0.2f;
    private int _burstCount = 1;
    private bool _canFire = true;
    private int _healthPoint = 100;
    private int _lives = 2;
    private EnemySpawnManager _enemySpawnManager;

    private float _boardLowerBorder = 3.5f;
    private float _boardUpperBorder = -3.5f;
    private float _boardRightBorder = 2.5f;
    private float _boardLeftBorder = -2.5f;

    [SerializeField] 
    public GameObject JetPlayer;
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    public float MoveUnit;
    [SerializeField]
    private GameObject _containerTypeBullet;

    // Start is called before the first frame update
    void Start()
    {
        _enemySpawnManager = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawnManager>();

        // Check if _enemySpawnManger is null.
        if (_enemySpawnManager == null ) 
        {
            Debug.LogError("Cannot find instance for EnemySpawnManager.");
        }
    }

    // Update is called once per frame
    void Update()
    {

        IEnumerator BulletTimer()
        {
            yield return new WaitForSecondsRealtime(_fireInterval);
            _canFire = true;
        }

        if (_canFire)
        {
            for (int i = 0; i < _burstCount; i++)
            {
                GameObject bullet = Instantiate(_bulletPrefab, transform.position + new Vector3(0f, 0.25f, 0f), Quaternion.identity);
                bullet.transform.parent = _containerTypeBullet.transform;
            }
            _canFire = false;
            StartCoroutine(BulletTimer());
        }

        transform.Translate(BoundaryCheckAndMove());

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If other is enemy, then damage the player and destroy (for now) the enemy.
        if (other.tag == "Enemy")
        {
            int damagePoint = 50;
            Destroy(other.gameObject);
            Damage(damagePoint);
        }
    }

    private Vector3 BoundaryCheckAndMove()
    {
        float _yMovement = Input.GetAxisRaw("Vertical");
        float _xMovement = Input.GetAxisRaw("Horizontal");
        float _yPosition = transform.position.y;
        float _xPosition = transform.position.x;

        if (_xPosition < _boardRightBorder && _xPosition > _boardLeftBorder &&
            _yPosition < _boardLowerBorder && _yPosition > _boardUpperBorder)
        {
            return (Vector3.up * MoveUnit * _yMovement * Time.deltaTime) + (Vector3.right * MoveUnit * _xMovement * Time.deltaTime);
        }
        else if (_xPosition <= _boardLeftBorder || _xPosition >= _boardRightBorder ||
            _yPosition >= _boardLowerBorder || _yPosition <= _boardUpperBorder)
        {
            // Debug.Log(string.Format("Updating JetPlayer intended as: x {0}, y {1}, horizontalMovement {2}, verticalMovement {3}", _xPosition, _yPosition, _xMovement, _yMovement));
            if ((_xPosition <= _boardLeftBorder || _xPosition >= _boardRightBorder) &&
                _xMovement * _xPosition > 0f)
            {
                _xMovement = 0f;
            }
            if ((_yPosition <= _boardUpperBorder || _yPosition >= _boardLowerBorder) &&
                _yMovement * _yPosition > 0f)
            {
                _yMovement = 0f;
            }
            return (Vector3.up * MoveUnit * _yMovement * Time.deltaTime) + (Vector3.right * MoveUnit * _xMovement * Time.deltaTime);
        }
        return new Vector3(0f, 0f, 0f);
    }

    void Damage(int damage)
    {
        _healthPoint -= damage;
        if (_healthPoint < 1)
        {
            _healthPoint = 100;
            _lives -= 1;
        }
        if (_lives < 1)
        {
            _enemySpawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
        Debug.Log(string.Format("Updating JetPlayer health and lives with damage as: damage {0}, HP {1}, lives {2}", damage, _healthPoint, _lives));
    }
}
