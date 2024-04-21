using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMidBossBulletLogic : MonoBehaviour, IBullet
{
    private int _angle = 0;

    [SerializeField] private float _speed = 8.0f;
    [SerializeField] private int _damage = 75;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector = Vector3.down * _speed * Time.deltaTime;
        vector = Quaternion.Euler(0, 0, _angle) * vector;
        transform.Translate(vector);

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
}
