using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPlayerController : MonoBehaviour
{

    [SerializeField] 
    public GameObject JetPlayer;
    [SerializeField]
    private GameObject _bulletPrefab;
    private float _fireInterval = 0.2f;
    private int _burstCount = 1;
    private bool _canFire = true;
    [SerializeField]
    public float MoveUnit;

    private float _boardLowerBorder = 3.5f;
    private float _boardUpperBorder = -3.5f;
    private float _boardRightBorder = 2.5f;
    private float _boardLeftBorder = -2.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    private Vector3 BoundaryCheckAndMove()
    {
        float _yMovement = Input.GetAxisRaw("Vertical");
        float _xMovement = Input.GetAxisRaw("Horizontal");
        float _yPosition = transform.position.y;
        float _xPosition = transform.position.x;

        if (_xPosition < _boardRightBorder && _xPosition > _boardLeftBorder && 
            _yPosition < _boardLowerBorder && _yPosition > _boardUpperBorder)
        {
            return (Vector3.up * MoveUnit * _yMovement * Time.deltaTime) + (Vector3.right * MoveUnit * _xMovement * Time.deltaTime);
        } else if (_xPosition <= _boardLeftBorder || _xPosition >= _boardRightBorder || 
            _yPosition >= _boardLowerBorder || _yPosition <= _boardUpperBorder)
        {
            // Debug.Log(string.Format("Updating JetPlayer intended as: x {0}, y {1}, horizontalMovement {2}, verticalMovement {3}", _xPosition, _yPosition, _xMovement, _yMovement));
            if ((_xPosition <= _boardLeftBorder || _xPosition >= _boardRightBorder) && 
                _xMovement * _xPosition > 0f)
            {
                _xMovement = 0f;
            }
            if ((_yPosition <= _boardUpperBorder || _yPosition >= _boardLowerBorder) && 
                _yMovement * _yPosition > 0f)
            {
                _yMovement = 0f;
            }
            return (Vector3.up * MoveUnit * _yMovement * Time.deltaTime) + (Vector3.right * MoveUnit * _xMovement * Time.deltaTime);
        }
        return new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

        IEnumerator BulletTimer()
        {
            yield return new WaitForSecondsRealtime(_fireInterval);
            _canFire = true;
        }

        if (_canFire)
        {
            for (int i = 0; i < _burstCount; i++)
            {
                Instantiate(_bulletPrefab, transform.position + new Vector3(0f, 0.25f, 0f), Quaternion.identity);
            }
            _canFire = false;
            StartCoroutine(BulletTimer());
        }

        transform.Translate(BoundaryCheckAndMove());

    }
}
