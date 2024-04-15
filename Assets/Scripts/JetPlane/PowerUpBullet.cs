using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBullet : AbstractPowerUp
{
    void Start()
    {
        _speed = 3.0f;
    }

    public override void ProcessPowerUp(JetPlayerController player)
    {
        Debug.Log("From PowerUpBullet as " + this);
        player.Burst += 2;
    }

    // Update is called once per frame
    void Update()
    {
        ControlMovement();
    }

}
