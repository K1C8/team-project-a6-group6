// Inspired by tutorial: https://medium.com/@dhunterthornton

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JetPlayerController : MonoBehaviour, IExplosible
{
    private float _fireInterval = 0.2f;
    private int _burstCount = 1;
    private bool _canFire = true;
    private bool _canDodge = true;
    private int _hp = 100;
    private int _lives = 2;
    private int _observerPointer = 0;
    private int _observerWindow = 10;
    private bool _isInvincible = false;
    private SpriteRenderer _spriteRenderer;
    private JetSpawnManager _enemySpawnManager;

    private bool _isKeyUpPressed = false;
    private bool _isKeyDownPressed = false;
    private bool _isKeyLeftPressed = false;
    private bool _isKeyRightPressed = false;
    private float _boardLowerBorder = 2.6f;
    private float _boardUpperBorder = -2.6f;
    private float _boardRightBorder = 2.05f;
    private float _boardLeftBorder = -2.05f;
    private float _dodgeCoolDownTime = 2f;
    private float _dodgeMinimumDelta = 0.6f;
    private float _dodgeSpeed = -4.0f;
    private float _dodgeTime = 0.25f;
    private float _dodgeVectorX = 0.0f;
    private float _hitInvincibleCoolDownTime = 3.0f;
    private float[] _observedXAccel;
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
    [SerializeField]
    private JetGameManagerLogic _jetGameManagerLogic;
    //[SerializeField] 

    public int Burst
    {
        get { return _burstCount; }
        set
        {
            if (_burstCount <= 3) 
            {
                _burstCount = value;
                Debug.Log(string.Format("JetPlayerController has burst count increased to {0}", _burstCount));
            }
        }
    }

    public int Hp
    {
        get { return _hp; }
        set
        {
            //_jetGameManagerLogic.PlayerHp = value;
            if (_lives > 0 && value < 251)
            {
                _hp = value;
            }
            else
            {
                _hp = 0;
            }
        }
    }

    public int Lives
    {
        get { return _lives; }
        set
        {
            //_jetGameManagerLogic.PlayerLives = value; 
            if (value < 5)
            {
                _lives = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AccelInitialize();
        _jetGameManagerLogic.Player = this;
        _enemySpawnManager = GameObject.Find("EnemySpawnManager").GetComponent<JetSpawnManager>();
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        // Check if _enemySpawnManger is null.
        if (_enemySpawnManager == null ) 
        {
            Debug.LogError("Cannot find instance for EnemySpawnManager.");
        }

        // Do not reverse these two lines, as PlayerHp has logic to set to 0 if PlayerLives is 0.
        // _jetGameManagerLogic.PlayerLives = _lives;
        // _jetGameManagerLogic.PlayerHp = _hp;
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

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("JetPlayerBullet");
            }
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
        AccelInputProcess(ObserverX());
        transform.Translate(BoundaryCheckAndMove());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        // If the other object is a bullet from an enemy, destroy the enemy bullet then take the damage.
        if (other.CompareTag("Enemy"))
        {
            IBullet bullet = null;
            switch (other.name)
            {
                case "EnemyEntryBulletPrefab(Clone)":
                    {
                        bullet = otherObject.GetComponent<EnemyEntryBulletLogic>();
                        break;
                    }
                case "EnemyMidBossBulletPrefab(Clone)":
                    {
                        bullet = otherObject.GetComponent<EnemyMidBossBulletLogic>();
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
        } else if (other.CompareTag("PowerUp"))
        {

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("JetPlayerPowerUp");
            }
            AbstractPowerUp powerUp = null;
            switch (other.name)
            {
                case "PowerUpBulletPrefab(Clone)":
                    {
                        Debug.Log("PowerUp Bullet collided.");
                        powerUp = otherObject.GetComponent<PowerUpBullet>();
                        break;
                    }
                case "PowerUpHpPrefab(Clone)":
                    {
                        Debug.Log("PowerUp HP collided.");
                        powerUp = otherObject.GetComponent<PowerUpHp>();
                        break;
                    }
                case "PowerUpLivesPrefab(Clone)":
                    {
                        Debug.Log("PowerUp Lives collided.");
                        powerUp = otherObject.GetComponent<PowerUpLives>();
                        break;
                    }
                default : break;
            }
            if (powerUp != null)
            {
                //Debug.Log("From JetPlayerController as " + this);
                powerUp.ProcessPowerUp(this);
                Destroy(other.gameObject);
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
        if (System.Math.Abs(_dodgeVectorX) > 0.1f)
        {
            _xMovement = _dodgeVectorX;
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
        _hp -= damage;
        if (_hp < 1)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("JetPlayerExplosion");
            }
            _lives -= 1;
            _hp = 100;
            ResetPowerUp();
            InvincibleEffect(_hitInvincibleCoolDownTime);
        }
        Debug.Log(string.Format("Updating JetPlayer health and lives with damage as: damage {0}, HP {1}, lives {2}", damage, _hp, _lives));
        // Do not reverse these two lines, as PlayerHp has logic to set to 0 if PlayerLives is 0.
        // _jetGameManagerLogic.PlayerLives = _lives;
        // _jetGameManagerLogic.PlayerHp = _hp;
        if (_lives < 1)
        {
            _hp = 0;
            _enemySpawnManager.OnPlayerDeath();
            _jetGameManagerLogic.OnGameOver();
            Destroy(this.gameObject);
        }
    }

    IEnumerator InvincibleTimer(float invincibleCoolDownTime)
    {
        yield return new WaitForSeconds(invincibleCoolDownTime);
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

    void InvincibleEffect(float invincibleCoolDownTime)
    {
        _playerCollider2D = this.gameObject.GetComponent<PolygonCollider2D>();
        _playerCollider2D.enabled = false; 
        _isInvincible = true;
        StartCoroutine(InvincibleTimer(invincibleCoolDownTime));
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
        //Debug.Log("Left Key Down called.");
    }

    public void OnLeftKeyUp()
    {
        _isKeyLeftPressed = false;
        //Debug.Log("Left Key Up called.");
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

    IEnumerator AfterDodge()
    {
        yield return new WaitForSeconds(_dodgeCoolDownTime);
        _canDodge = true;
        _jetGameManagerLogic.HpAndLivesTextColor = Color.white;
    }

    IEnumerator PerformDodge()
    {
        yield return new WaitForSeconds(_dodgeTime);
        _jetGameManagerLogic.HpAndLivesTextColor = Color.yellow;
        _dodgeVectorX = 0.0f;
    }

    void Dodge(float deltaX)
    {
        if (_canDodge) 
        { 
            // _tempAccel.color = Color.red;
            _jetGameManagerLogic.HpAndLivesTextColor = Color.red;
            // Calculate the direction of the dodge, if deltaX is positive, then dodge to right;
            _dodgeVectorX = _dodgeSpeed * deltaX / System.Math.Abs(deltaX);
            Debug.Log("User dodging gesture detected.");
            _canDodge = false;
            StartCoroutine(PerformDodge());
            StartCoroutine(AfterDodge());
            InvincibleEffect(_dodgeTime);
        }
    }

    float ObserverX()
    {
        float accelX = Input.acceleration.x;
        float avgX = 0;
        float deltaX = accelX - avgX;
        foreach (float x in _observedXAccel)
        {
            avgX += (x / _observerWindow);
        }
        if (System.Math.Abs(deltaX) > _dodgeMinimumDelta)
        {
            // User may have performed a dodge.
            Dodge(deltaX);
        }
        _observedXAccel[_observerPointer] = accelX;
        _observerPointer += 1;
        _observerPointer %= _observerWindow;
        return deltaX;
    }

    void AccelInitialize()
    {
        _observedXAccel = new float[_observerWindow];
        float accelX = Input.acceleration.x;
        for(int i = 0; i < _observerWindow; i ++)
        {
            _observedXAccel[i] = accelX;
        }
    }

    void AccelInputProcess(float deltaX)
    {
        Vector3 accel = Input.acceleration;
        float x = accel.x;
        // Vector3 unityConversion = ConvertGyroInput(gyro);
        // string hintText = "Raw accel input:\n" + accel.ToString() + "\nDodge observed:\n" + _dodgeVectorX.ToString("0.0");
    }

}
