using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiSimulator : MonoBehaviour
{
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] GameObject player3;
    [SerializeField] GameObject player4;
    [SerializeField] GameObject UIcontroller;
    public void Start()
    {
        player1.GetComponentInChildren<TMP_Text>().text = "YOU";
        if (MultiSingleManager.Instance.isMulti)
        {
            player1.SetActive(true);
            player2.SetActive(true);
            player3.SetActive(true);
            player4.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            player1.SetActive(true);
            player2.SetActive(false);
            player3.SetActive(false);
            player4.SetActive(false);
        }
    }
}
