using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHp : AbstractPowerUp
{
    void Start()
    {
        _speed = 3.0f;
    }

    public override void ProcessPowerUp(JetPlayerController player)
    {
        Debug.Log("From PowerUpHp as " + this);
        player.Hp += 50;
    }

    // Update is called once per frame
    void Update()
    {
        ControlMovement();
    }

}
