using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPlayerController : MonoBehaviour
{

    [SerializeField] 
    public GameObject JetPlayer;
    public float MoveUnit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float _verticalMovement = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * MoveUnit * _verticalMovement * Time.deltaTime);
        Debug.Log("Updating JetPlayer");
    }
}
