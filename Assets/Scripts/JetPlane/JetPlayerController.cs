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

    private Vector3 BoundaryCheckAndMove(float x, float y, float horizontalMovement, float verticalMovement)
    {
        if (x < 2.5 && x > -2.5 && y < 4.5 && y > -4.5)
        {
            return (Vector3.up * MoveUnit * verticalMovement * Time.deltaTime) + (Vector3.right * MoveUnit * horizontalMovement * Time.deltaTime);
        } else if (x <= -2.5 || x >= 2.5 || y >= 4.5 || y <= -4.5)
        {
            Debug.Log(string.Format("Updating JetPlayer intended as: x {0}, y {1}, horizontalMovement {2}, verticalMovement {3}", x, y, horizontalMovement, verticalMovement));
            if ((x <= -2.5 || x >= 2.5) && horizontalMovement * x > 0f)
            {
                horizontalMovement = 0f;
            }
            if ((y <= -4.5 || y >= 4.5) && verticalMovement * y > 0f)
            {
                verticalMovement = 0f;
            }
            return (Vector3.up * MoveUnit * verticalMovement * Time.deltaTime) + (Vector3.right * MoveUnit * horizontalMovement * Time.deltaTime);
        }
        return new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float _verticalMovement = Input.GetAxisRaw("Vertical");
        float _horizontalMovement = Input.GetAxisRaw("Horizontal");
        float _verticalPosition = transform.position.y;
        float _horizontalPosition = transform.position.x;
        transform.Translate(BoundaryCheckAndMove(_horizontalPosition, _verticalPosition, _horizontalMovement, _verticalMovement));
    }
}
