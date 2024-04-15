using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntryBulletLogic : MonoBehaviour, IBullet
{
    private int _damage = 50;
    private int _angle = 0;

    [SerializeField]
    private float _speed = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -4f)
        {
            Destroy(gameObject, 1);
        }
    }

    public int GetDamage()
    {
        return _damage;
    }

    public void SetAngle(int angle)
    {
        _angle = angle;
    }

    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        // If the other object is the player, deduct the hit point of the player then destroy this bullet instance.
        if (other.tag == "Player")
        {
            IExplosible player = null;
            switch (other.name)
            {
                case "JetPlayer":
                    {
                        player = otherObject.GetComponent<JetPlayerController>();
                        break;
                    }
                default:
                    break;
            }
            if (player != null )
            {
                Debug.Log("JetPlayerController collided.");
            }
        }
    }*/
}
