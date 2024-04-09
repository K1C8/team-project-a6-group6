using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyEntryLogic : MonoBehaviour, IExplosible
{
    private float _xStart;
    private float _xPosition;
    private float _yPosition;
    private float _xLeftBound;
    private float _xRightBound;
    private float _yLowerBound = 2.5f;
    private Vector3 _spawnPosition;
    private int _direction = 1;

    [SerializeField]
    private float _boardLeftBorder = -2.5f;
    [SerializeField]
    private float _boardRightBorder = 2.5f;
    [SerializeField]
    private float _speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        ControlMovement();
    }

    void Spawn()
    {
        _xStart = transform.position.x;
        _xLeftBound = Math.Max(_boardLeftBorder, _xStart - 1);
        _xRightBound = Math.Min(_boardRightBorder, _xStart + 1);

        _xPosition = transform.position.x;
        _spawnPosition = new Vector3(_xPosition, 4, 0);
        transform.position = _spawnPosition;


    }

    void ControlMovement()
    {
        IEnumerator DirectionDecision()
        {
            yield return new WaitForSecondsRealtime(0.2f);
            _xPosition = transform.position.x;
            if (_xPosition < _xLeftBound) 
            {
                _direction = 1;
            } else if (_xPosition > _xRightBound)
            {
                _direction = -1;
            }
        }

        _xPosition = transform.position.x;
        _yPosition = transform.position.y;

        StartCoroutine(DirectionDecision());

        if (_yPosition > _yLowerBound)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        } else
        {
            transform.Translate(Vector3.right * _speed * _direction * Time.deltaTime);
        }
        // Debug.Log(string.Format("Updating EnemyEntry intended as: x {0}, y {1}, direction {2}", _xPosition, _yPosition, _direction));
    }

    public int GetDamage()
    {
        return 50;
    }

    public int GetHealth()
    {
        return 100;
    }
}
