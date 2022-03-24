using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private static readonly string DAMAGEABLE = "Damageable";
    private static readonly string ENVIRONMENT = "Environment";
    private static readonly string PLAYER = "Player";
    private string _senderTag;

    private Rigidbody2D _rb;
    private CapsuleCollider2D _collider;
    private ObjectPool<Bullet> _originPool;

    private int _damage;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    public void Init(ObjectPool<Bullet> bulletPool, int damage, string senderTag)
    {
        _originPool = bulletPool;
        _damage = damage;
        _senderTag = senderTag;
    }

    public void Shot(Vector3 position, Vector3 direction, Vector2 velocity)
    {
        transform.position = position;
        transform.Rotate(new Vector3(0.0f, 0.0f, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90.0f));
        _rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(ENVIRONMENT))
        {
            _originPool.Release(this);
        }

        if (DAMAGEABLE != _senderTag)
            if (collision.gameObject.CompareTag(DAMAGEABLE))
            {
                var target = collision.gameObject.GetComponentInParent(typeof(IDamageable)) as IDamageable;
                target.TakeDamage(_damage);
                _originPool.Release(this);
            }

        if (PLAYER != _senderTag)
            if (collision.gameObject.CompareTag(PLAYER))
            {
                var target = collision.gameObject.GetComponentInParent(typeof(IDamageable)) as IDamageable;
                target.TakeDamage(_damage);
                _originPool.Release(this);
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(ENVIRONMENT))
        {
            _originPool.Release(this);
        }
    }
}
