using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy: MonoBehaviour, IDamageable
{
    [SerializeField]
    private Transform _model;

    public abstract string enemyName { get; }
    public EnemyFactory OriginFactory { get; set; }

    private EnemyBaseBehaviour behaviour;
    protected Rigidbody2D rb;
    protected NavMeshAgent _navMeshAgent;
    protected StateMachine _stateMachine;
    private IDamageable _target;

    protected Vector2 spawnPosition;

    public float Health { get; private set; }
    public float Scale { get; private set; }
    public float PatrolRange { get; private set; }
    public float AggroRange { get; private set; }

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _stateMachine = new StateMachine();
    }

    public virtual void Init(float health, float scale, float patrolRange, float aggroRange)
    {
        Health = health;
        Scale = scale;
        PatrolRange = patrolRange;
        AggroRange = aggroRange;
        _model.localScale = new Vector3(scale, scale, scale);
        //behaviour = GetComponent<EnemyBaseBehaviour>();
        //behaviour.Init(this);
    }

    public void Update()
    {
        _stateMachine.Tick();
    }

    public virtual void Move()
    {
        //behaviour.Move();
    }

    public virtual void Attack()
    {
        //behaviour.Attack();
    }

    public virtual void Spawn(Vector3 position)
    {
        //transform.position = position;
        _navMeshAgent.Warp(position);
        spawnPosition = position;
        //Move();
    }

    public virtual void TakeDamage(float value)
    {
        Health -= value;
    }
}
