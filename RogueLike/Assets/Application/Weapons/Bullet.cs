using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CapsuleCollider2D _collider;
    private ObjectPool<Bullet> _originPool;

    private float _damage;
    private static readonly string _tag = "Damageable";
    private static readonly string _environmentTag = "Environment";

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    public void Init(ObjectPool<Bullet> bulletPool, float damage)
    {
        _originPool = bulletPool;
        _damage = damage;
    }

    public void Shot(Vector3 position, Vector3 direction, Vector2 velocity)
    {
        transform.position = position;
        transform.Rotate(new Vector3(0.0f, 0.0f, Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg));
        _rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(_tag))
        {
            var target = collision.gameObject.GetComponentInParent(typeof(IDamageable)) as IDamageable;
            target.TakeDamage(_damage);
            _originPool.Release(this);
        }

        if (collision.gameObject.CompareTag(_environmentTag))
        {
            _originPool.Release(this);
        }
    }
}
