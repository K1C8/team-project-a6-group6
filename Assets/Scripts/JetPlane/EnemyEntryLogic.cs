using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyEntryLogic : AbstractEnemyLogic
{
    private float _xStart;
    private float _xPosition;
    private float _yPosition; 
    private Vector3 _spawnPosition;
    // private int _direction = 1;
    // private int _hp = 50;
    // private int _score = 50;
    // private int _damage = 50;
    // private float _fireInterval = 3.0f;
    // private int _burstCount = 1;
    // private bool _canFire = true;
    // private GameObject _containerTypeEnemyBullet;
    // private float _timeToRush;

    [SerializeField] private float _boardLeftBorder = -2.05f;
    [SerializeField] private float _boardRightBorder = 2.05f;
    [SerializeField] private float _fireIntervalEntry = 3.0f;
    [SerializeField] private float _speedEntry = 2f;
    [SerializeField] private float _yLowerBoundEntry = 2.5f;
    [SerializeField] private float _yVisibleUpperBoundEntry = 2.7f;
    [SerializeField] private int _bulletAngleEntry = 5;
    [SerializeField] private int _minimumValueBulletEntry = 93;
    [SerializeField] private int _minimumValueHpEntry = 98;
    [SerializeField] private int _minimumValueExtraLifeEntry = 99;
    [SerializeField] private int _minimumValueShieldEntry = 100;
    [SerializeField] private GameObject _enemyEntryBullet;
    [SerializeField] private GameObject _powerUpBulletEntry;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
        SetUpEnemyBulletContainer();
        SpawnInvincible();
    }

    // Update is called once per frame
    void Update()
    {
        ActionControl(_enemyEntryBullet);
    }

    protected void Spawn()
    {
        _fireInterval = _fireIntervalEntry;
        _speed = _speedEntry;
        _timeToRush = UnityEngine.Random.Range(2f, 6f);
        // _yLowerBound = 2.5f;

        _bulletAngle = _bulletAngleEntry;
        _burstCount = 1;
        _damage = 50;
        _direction = 1;
        _hp = 50;
        _minimumValueBullet = _minimumValueBulletEntry;
        _minimumValueHp = _minimumValueHpEntry;
        _minimumValueExtraLife = _minimumValueExtraLifeEntry;
        _minimumValueShield = _minimumValueShieldEntry;
        _score = 50;
        _canFire = true;
        _isTimeToRush = false;

        _xStart = transform.position.x;
        _xLeftBound = Math.Max(_boardLeftBorder, _xStart - 1);
        _xRightBound = Math.Min(_boardRightBorder, _xStart + 1);
        _yLowerBound = _yLowerBoundEntry;
        _yVisibleUpperBound = _yVisibleUpperBoundEntry;

        _xPosition = transform.position.x;
        _spawnPosition = new Vector3(_xPosition, 4, 0);
        transform.position = _spawnPosition;
        _powerUpBullet = _powerUpBulletEntry;
        _jetGameManager = GameObject.Find("JetGameManager").GetComponent<JetGameManagerLogic>();
        if (_jetGameManager == null)
        {
            Debug.LogWarning("Cannot find the instance of the JetGameMangerLogic!");
        }
    }

    /*
    IEnumerator DirectionDecision(float x)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        if (x < _xLeftBound)
        {
            _direction = 1;
        }
        else if (x > _xRightBound)
        {
            _direction = -1;
        }
    }

    IEnumerator BulletTimer()
    {
        yield return new WaitForSeconds(_fireInterval);
        _canFire = true;
    }

    IEnumerator RushToBottom(float timeToRush)
    {
        yield return new WaitForSeconds(timeToRush);
        _isTimeToRush = true;
    }

    void ActionControl(GameObject bulletObject)
    {
        if (transform.position.y > _yLowerBound)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        } 
        else
        {
            StartCoroutine(RushToBottom(_timeToRush));
            if (_isTimeToRush)
            {
                transform.Translate(Vector3.down * _speed * 2 * Time.deltaTime);
            }
            else
            {
                StartCoroutine(DirectionDecision(transform.position.x));
                transform.Translate(Vector3.right * _speed * _direction * Time.deltaTime);
            }

            if (_canFire)
            {
                _canFire = false;
                GameObject bullet = Instantiate(bulletObject, transform.position + new Vector3(0f, -0.25f, 0f), Quaternion.identity);
                if (_containerTypeEnemyBullet != null)
                {
                    bullet.transform.parent = _containerTypeEnemyBullet.transform;
                }
                StartCoroutine(BulletTimer());
            }
        }
        // Debug.Log(string.Format("Updating EnemyEntry intended as: x {0}, y {1}, direction {2}", transform.position.x, transform.position.y, _direction));
    }*/

    /*
    public int GetDamage()
    {
        return _damage;
    }

    public void TakeDamage(int damageTaken)
    {
        _hp -= damageTaken;
        Debug.Log(string.Format("Enemy Entry has {0} hit point left.", _hp));
        if (_hp < 1)
        {
            Destroy(this.gameObject);
        }
    }*/

    /*
    //Note: All collide calculation is done on the side which can take damage (aka active objects). If both sides are able to take damage, the logic is on the enemy side.
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        // If the other object is the player, deduct the hit point of the player then destroy this bullet instance.
        if (other.tag == "Player")
        {
            IExplosible player = null;
            PlayerBulletLogic bullet = null;
            int selfDamage = 0;
            switch (other.name)
            {
                case "JetPlayer":
                    {
                        player = otherObject.GetComponent<JetPlayerController>();
                        break;
                    }
                case "PlayerBulletPrefab(Clone)":
                    {
                        bullet = otherObject.GetComponent<PlayerBulletLogic>();
                        break;
                    }
                default:
                    break;
            }
            if (player != null)
            {
                Debug.Log("JetPlayerController found.");
                selfDamage = player.GetDamage();
                player.TakeDamage(_damage);
                TakeDamage(selfDamage);
            } else if (bullet != null)
            {
                selfDamage = bullet.GetDamage();
                Destroy(otherObject);
                TakeDamage(selfDamage);
            }
        }
    }
    */
}
