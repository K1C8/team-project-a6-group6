using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPowerUp : MonoBehaviour
{

    protected float _speed;

    protected void ControlMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }

    public abstract void ProcessPowerUp(JetPlayerController player);
}
