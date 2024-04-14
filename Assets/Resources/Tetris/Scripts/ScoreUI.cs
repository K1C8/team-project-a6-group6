using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private TMP_Text myText;
    public Board myBoard;


    public void Start()
    {
        myText = GetComponent<TMP_Text>();
    }

    public void Update()
    {
        myText.text = myBoard.score.ToString();

        if (myBoard.isGameOver)
        {
            myText.text = myBoard.score.ToString() + "\nGame over!";
        }
    }
}
