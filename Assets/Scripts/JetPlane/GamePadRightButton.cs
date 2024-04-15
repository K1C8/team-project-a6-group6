using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamePadRightButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private JetPlayerController _playerController;

    public void OnPointerDown(PointerEventData eventData)
    {
        _playerController.OnRightKeyDown();
    }

    public void OnPointerUp(PointerEventData evenetData)
    {
        _playerController.OnRightKeyUp();
    }


}
