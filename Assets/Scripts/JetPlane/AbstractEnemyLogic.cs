using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemyLogic : MonoBehaviour, IExplosible
{
    protected float _fireInterval;
    protected float _minimumValueBullet;
    protected float _minimumValueHp;
    protected float _minimumValueExtraLife;
    protected float _minimumValueShield;
    protected float _speed;
    protected float _timeToRush;
    protected float _xLeftBound;
    protected float _xRightBound;
    protected float _yVisibleUpperBound;
    protected float _yLowerBound;
    protected int _bulletAngle;
    protected int _burstCount;
    protected int _damage;
    protected int _hp;
    protected int _direction;
    protected int _score;
    protected bool _canFire;
    protected bool _isInvincible;
    protected bool _isTimeToRush;
    protected Collider2D _collider;
    protected GameObject _containerTypeEnemyBullet;
    protected GameObject _powerUpBullet;
    protected GameObject _powerUpHp;
    protected GameObject _powerUpLives;
    protected JetGameManagerLogic _jetGameManager;

    protected void SetUpEnemyBullet()
    {
        _containerTypeEnemyBullet = GameObject.Find("EnemyBulletContainer");
        if (_containerTypeEnemyBullet == null)
        {
            Debug.LogWarning("Container for enemy bullets is null.");
        }
    }

    public int GetDamage()
    {
        return _damage;
    }

    public int GetScore()
    {
        return _score;
    }

    public void TakeDamage(int damageTaken)
    {
        _hp -= damageTaken;
        Debug.Log(string.Format("Enemy has {0} hit point left.", _hp));
        if (_hp < 1)
        {
            AudioManager.Instance.PlaySFX("JetEnemyExplosion");
            float diceResult = RollPowerUpDice();
            if (diceResult >= _minimumValueExtraLife && _powerUpLives != null)
            {
                Instantiate(_powerUpLives, transform.position + new Vector3(0, -0.25f, 0), Quaternion.identity);
            }
            else if (diceResult >= _minimumValueHp && _powerUpHp != null)
            {
                Instantiate(_powerUpHp, transform.position + new Vector3(0, -0.25f, 0), Quaternion.identity);
            }
            else if (diceResult >= _minimumValueBullet && _powerUpBullet != null)
            {
                Instantiate(_powerUpBullet, transform.position + new Vector3(0, -0.25f, 0), Quaternion.identity);
            }
            if (_jetGameManager != null)
            {
                _jetGameManager.PlayerScore += _score;
            } else
            {
                Debug.LogWarning("Cannot find JetGameMangerLogic instance!");
            }
            Destroy(this.gameObject);
        }
        else
        {
            AudioManager.Instance.PlaySFX("JetEnemyHit");
        }
    }

    protected IEnumerator RushToBottom(float timeToRush)
    {
        yield return new WaitForSeconds(timeToRush);
        _isTimeToRush = true;
    }

    protected IEnumerator BulletTimer()
    {
        yield return new WaitForSeconds(_fireInterval);
        _canFire = true;
    }

    protected IEnumerator DirectionDecision(float x)
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

    protected void ActionControl(GameObject bulletObject)
    {
        // This enemy object has entered the visible area of the player.
        if (transform.position.y < _yVisibleUpperBound && _isInvincible)
        {
            CeaseInvincible();
        }

        // The enemy has yet to arrive the 'stopping' range.
        if (transform.position.y > _yLowerBound)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        // After entered the 'stopping' range, the enemy will move horizontally until run out of the preconfigured waiting time to rush downward.
        else
        {
            StartCoroutine(DirectionDecision(transform.position.x));
            StartCoroutine(RushToBottom(_timeToRush));
            if (_isTimeToRush)
            {
                transform.Translate(Vector3.down * _speed * 2 * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.right * _speed * _direction * Time.deltaTime);
            }

            if (_canFire)
            {
                AudioManager.Instance.PlaySFX("JetEnemyBullet");
                _canFire = false;
                int angleWidthTotal = _bulletAngle * (_burstCount - 1);
                int angle = angleWidthTotal / -2;
                for (int i = 0; i < _burstCount; i++)
                {
                    GameObject bullet = Instantiate(bulletObject, transform.position + new Vector3(0f, -0.25f, 0f), Quaternion.identity);
                    IBullet bulletLogic = null;
                    switch (name)
                    {
                        case "EnemyMidBossPrefab(Clone)":
                            bulletLogic = bullet.GetComponent<EnemyMidBossBulletLogic>();
                            break;
                        case "EnemyEntryPrefab(Clone)":
                            bulletLogic = bullet.GetComponent<EnemyEntryBulletLogic>();
                            break;
                        default: break;
                    }
                    // IBullet bulletLogic = bullet.GetComponent<EnemyEntryBulletLogic>();
                    if (bulletLogic != null)
                    {
                        bulletLogic.SetAngle(angle);
                    }
                    angle += _bulletAngle;
                    bullet.transform.parent = _containerTypeEnemyBullet.transform;
                }
                StartCoroutine(BulletTimer());
            }
            if (transform.position.y < -10f)
            {
                Destroy(this.gameObject);
            }
        }
        // Debug.Log(string.Format("Updating EnemyEntry intended as: x {0}, y {1}, direction {2}", transform.position.x, transform.position.y, _direction));
    }

    //Note: All collide calculation is done on the side which can take damage (aka active objects). If both sides are able to take damage, the logic is on the enemy side.
    protected void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        // If the other object is the player, deduct the hit point of the player then destroy this bullet instance.
        if (other.CompareTag("Player"))
        {
            IExplosible player = null;
            PlayerBulletLogic bullet = null;
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
            int selfDamage;
            if (player != null)
            {
                Debug.Log("JetPlayerController found.");
                selfDamage = player.GetDamage();
                player.TakeDamage(_damage);
                TakeDamage(selfDamage);
            }
            else if (bullet != null)
            {
                selfDamage = bullet.GetDamage();
                Destroy(otherObject);
                TakeDamage(selfDamage);
            }
        }
    }

    private float RollPowerUpDice()
    {
        return Random.Range(0f, 100.05f);
    }

    // Make the enemy invincible before being scrolled into the visible area.
    protected void SpawnInvincible()
    {
        _collider = this.gameObject.GetComponent<BoxCollider2D>();
        _collider.enabled = false;
        _isInvincible = true;
    }

    protected void CeaseInvincible()
    {
        _collider = this.gameObject.GetComponent<BoxCollider2D>();
        _collider.enabled = true;
        _isInvincible = false;
    }
}
