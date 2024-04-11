using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBullet : AbstractPowerUp
{
    public override void ProcessPowerUp(JetPlayerController player)
    {
        player.Burst += 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
