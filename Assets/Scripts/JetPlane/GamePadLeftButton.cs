using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamePadLeftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private JetPlayerController _playerController;

    public void OnPointerDown(PointerEventData eventData)
    {
        _playerController.OnLeftKeyDown();
    }

    public void OnPointerUp(PointerEventData evenetData)
    {
        _playerController.OnLeftKeyUp();
    }


}
