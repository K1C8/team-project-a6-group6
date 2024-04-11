using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemyLogic : MonoBehaviour, IExplosible
{
    private int _damage;
    private int _hitPoint;
    private bool _isTimeToRush;

    public int GetDamage()
    {
        return _damage;
    }

    public void TakeDamage(int damageTaken)
    {
        _hitPoint -= damageTaken;
        Debug.Log(string.Format("Enemy Entry has {0} hit point left.", _hitPoint));
        if (_hitPoint < 1)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator RushToBottom(float timeToRush)
    {
        yield return new WaitForSeconds(timeToRush);
        _isTimeToRush = true;
    }
}
