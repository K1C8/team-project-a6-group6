using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpLives : AbstractPowerUp
{
    void Start()
    {
        _speed = 3.0f;
    }

    public override void ProcessPowerUp(JetPlayerController player)
    {
        Debug.Log("From PowerUpLives as " + this);
        player.Lives++;
    }

    // Update is called once per frame
    void Update()
    {
        ControlMovement();
    }

}
