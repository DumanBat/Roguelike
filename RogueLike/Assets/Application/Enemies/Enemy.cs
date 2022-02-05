using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy: MonoBehaviour
{
    public abstract string enemyName { get; }
    public EnemyFactory OriginFactory { get; set; }

    private EnemyBaseBehaviour behaviour;
    public Rigidbody2D rb;

    public Vector2 spawnPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Init()
    {
        behaviour = GetComponent<EnemyBaseBehaviour>();
        behaviour.Init(this);
    }

    public virtual void Move()
    {
        behaviour.Move();
    }

    public virtual void Attack()
    {
        behaviour.Attack();
    }

    public virtual void Spawn(Vector3 position)
    {
        transform.position = position;
        spawnPosition = position;
        Move();
    }
}
