using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPlayerController : MonoBehaviour, IExplosible
{
    private float _fireInterval = 0.2f;
    private int _burstCount = 1;
    private bool _canFire = true;
    private int _healthPoint = 100;
    private int _lives = 2;
    private bool _isInvincible = false;
    private SpriteRenderer _spriteRenderer;
    private EnemySpawnManager _enemySpawnManager;

    private bool _isKeyUpPressed = false;
    private bool _isKeyDownPressed = false;
    private bool _isKeyLeftPressed = false;
    private bool _isKeyRightPressed = false;
    private float _boardLowerBorder = 2.6f;
    private float _boardUpperBorder = -2.6f;
    private float _boardRightBorder = 2.5f;
    private float _boardLeftBorder = -2.5f;
    private float _invincibleCoolDownTime = 3.0f;
    private Collider2D _playerCollider2D;
    private int _bulletAngle = 5;

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
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

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
            int angleWidthTotal = _bulletAngle * (_burstCount - 1);
            int angle = angleWidthTotal / -2;
            for (int i = 0; i < _burstCount; i++)
            {
                GameObject bullet = Instantiate(_bulletPrefab, transform.position + new Vector3(0f, 0.25f, 0f), Quaternion.identity);
                IBullet bulletLogic = bullet.GetComponent<PlayerBulletLogic>();
                bulletLogic.SetAngle(angle);
                angle += _bulletAngle;
                bullet.transform.parent = _containerTypeBullet.transform;
            }
            _canFire = false;
            StartCoroutine(BulletTimer());
        }

        transform.Translate(BoundaryCheckAndMove());
        // ResetKeyPressed();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        // If the other object is a bullet from an enemy, destroy the enemy bullet then take the damage.
        if (other.tag == "Enemy")
        {
            IBullet bullet = null;
            switch (other.name)
            {
                case "EnemyEntryBulletPrefab(Clone)":
                    {
                        bullet = otherObject.GetComponent<EnemyEntryBulletLogic>();
                        break;
                    }
                default:
                    break;
            }
            if (bullet != null)
            {
                Debug.Log("Bullet from enemy collided.");
                int selfDamage = bullet.GetDamage();
                Destroy(other.gameObject);
                TakeDamage(selfDamage);
            }
        }
    }

    public int GetDamage()
    {
        return 100;
    }

    public void TakeDamage(int damageTaken)
    {
        Damage(damageTaken);
    }

    private Vector3 BoundaryCheckAndMove()
    {
        float _yMovement = Input.GetAxisRaw("Vertical");
        float _xMovement = Input.GetAxisRaw("Horizontal");
        if (_isKeyUpPressed)
        {
            _yMovement += 1.0f;
        }
        if (_isKeyDownPressed)
        {
            _yMovement -= 1.0f;
        }
        if (_isKeyLeftPressed)
        {
            _xMovement -= 1.0f;
        }
        if (_isKeyRightPressed)
        {
            _xMovement += 1.0f;
        }
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
            ResetPowerUp();
            InvincibleEffect();
        }
        if (_lives < 1)
        {
            _enemySpawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
        Debug.Log(string.Format("Updating JetPlayer health and lives with damage as: damage {0}, HP {1}, lives {2}", damage, _healthPoint, _lives));
    }

    IEnumerator InvincibleTimer()
    {
        yield return new WaitForSeconds(_invincibleCoolDownTime);
        _playerCollider2D.enabled = true;
        _isInvincible = false;
        _spriteRenderer.enabled = true;
    }

    IEnumerator InvincibleFlicker()
    {
        while (_isInvincible)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.25f);
        }
    }

    void InvincibleEffect()
    {
        _playerCollider2D = this.gameObject.GetComponent<PolygonCollider2D>();
        _playerCollider2D.enabled = false; 
        _isInvincible = true;
        StartCoroutine(InvincibleTimer());
        if (_isInvincible)
        {
            StartCoroutine(InvincibleFlicker());
        }
    }

    public void OnUpKeyDown()
    {
        _isKeyUpPressed = true;
    }

    public void OnUpKeyUp()
    {
        _isKeyUpPressed = false;
    }

    public void OnDownKeyDown()
    {
        _isKeyDownPressed = true;
    }

    public void OnDownKeyUp()
    {
        _isKeyDownPressed = false;
    }

    public void OnLeftKeyDown()
    {
        _isKeyLeftPressed = true;
        Debug.Log("Left Key Down called.");
    }

    public void OnLeftKeyUp()
    {
        _isKeyLeftPressed = false;
        Debug.Log("Left Key Up called.");
    }

    public void OnRightKeyDown()
    {
        _isKeyRightPressed = true;
    }

    public void OnRightKeyUp()
    {
        _isKeyRightPressed = false;
    }

    private void ResetPowerUp()
    {
        _burstCount = 1;
    }
}
