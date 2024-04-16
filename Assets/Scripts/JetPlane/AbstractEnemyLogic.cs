using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemyLogic : MonoBehaviour, IExplosible
{
    protected float _fireInterval;
    protected float _speed;
    protected float _timeToRush;
    protected float _xLeftBound;
    protected float _xRightBound;
    protected float _yVisibleUpperBound;
    protected float _yLowerBound;
    protected int _bulletAngle;
    protected int _burstCount;
    protected int _damage;
    protected int _hitPoint;
    protected int _minimumValueBullet;
    protected int _minimumValueHitPoint;
    protected int _minimumValueExtraLife;
    protected int _minimumValueShield;
    protected int _direction;
    protected int _score;
    protected bool _canFire;
    protected bool _isInvincible;
    protected bool _isTimeToRush;
    protected GameObject _containerTypeEnemyBullet;
    protected GameObject _powerUpBullet;
    protected Collider2D _collider;
    protected JetGameManagerLogic _jetGameManager;

    private int _diceResult = 0;

    protected void SetUpEnemyBulletContainer()
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
        _hitPoint -= damageTaken;
        Debug.Log(string.Format("Enemy Entry has {0} hit point left.", _hitPoint));
        if (_hitPoint < 1)
        {
            AudioManager.Instance.PlaySFX("JetEnemyExplosion");
            RollPowerUpDice();
            if (_diceResult >= _minimumValueBullet && _powerUpBullet != null)
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
                    IBullet bulletLogic = bullet.GetComponent<EnemyEntryBulletLogic>();
                    bulletLogic.SetAngle(angle);
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

    private void RollPowerUpDice()
    {
        _diceResult = Random.Range(0, 100);
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
