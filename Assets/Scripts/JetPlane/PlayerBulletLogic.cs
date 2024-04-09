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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the other object is an Enemy, destroy the enemy then destroy this bullet instance.
        if (other.tag == "Enemy")
        {
            if (other.gameObject is IExplosible)
            {
                Debug.Log("IExplosible instance caught.");
            }
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
