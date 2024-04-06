using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JetPlayerUpEvent : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    bool isPressed = false;
    public GameObject JetPlayer;
    public float MoveUnit;

    // Update is called once per frame
    void Update()
    {
        if (isPressed)
        {
            JetPlayer.transform.Translate(0, MoveUnit * Time.deltaTime, 0);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Up Pressed.");
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
