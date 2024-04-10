using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamePadUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private JetPlayerController _playerController;

    public void OnPointerDown(PointerEventData eventData)
    {
        _playerController.OnUpKeyDown();
    }

    public void OnPointerUp(PointerEventData evenetData)
    {
        _playerController.OnUpKeyUp();
    }


}
