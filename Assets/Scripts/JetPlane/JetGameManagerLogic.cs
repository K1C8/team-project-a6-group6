using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetGameManagerLogic : MonoBehaviour 
{
    private int _playerScore;

    public int PlayerScore
    {
        get { return _playerScore; }
        set { _playerScore = value; }
    }

    // Member to call when user touches a button
    public void PressToStart()
    {
        GameObject JetPlayer = GameObject.Find("JetPlayer");
        GameObject EnemySpawnManager = GameObject.Find("EnemySpawnManager");
        JetPlayer.SetActive(true);
        EnemySpawnManager.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
