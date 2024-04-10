using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosible
{
    public int GetDamage();

    public void TakeDamage(int damageTaken);
}
