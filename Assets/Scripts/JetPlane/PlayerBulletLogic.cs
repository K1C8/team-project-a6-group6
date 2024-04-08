using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletLogic : MonoBehaviour
{
    [SerializeField]
    private float _speed = 12.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 5f)
        {
            Destroy(gameObject, 1);
        }
    }
}
