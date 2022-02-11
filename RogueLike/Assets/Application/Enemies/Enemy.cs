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

    protected Rigidbody2D _rb;
    protected NavMeshAgent _navMeshAgent;
    [SerializeField]
    protected Animator _animator;
    protected StateMachine _stateMachine;
    [SerializeField]
    protected EnemyDetector _enemyDetector;

    protected Vector2 _spawnPosition;

    public float Health { get; private set; }
    public float Damage { get; private set; }
    public float Scale { get; private set; }
    public float PatrolRange { get; private set; }
    public float AggroRange { get; private set; }
    public float MeleeRange { get; private set; }
    public float AttackCooldown { get; private set; }
    public float AggroCooldown { get; private set; }

    public virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _stateMachine = new StateMachine();
    }

    public virtual void Init(float health, float damage, float scale, float patrolRange, 
        float aggroRange, float meleeRange, float attackCooldown, float aggroCooldown)
    {
        Health = health;
        Damage = damage;
        Scale = scale;
        PatrolRange = patrolRange;
        AggroRange = aggroRange;
        MeleeRange = meleeRange;
        AttackCooldown = attackCooldown;
        AggroCooldown = aggroCooldown;
        _enemyDetector.Init(aggroRange, meleeRange, aggroCooldown);
        _model.localScale = new Vector3(scale, scale, scale);

        Init();
    }

    public virtual void Init()
    {

    }

    public void Update()
    {
        _stateMachine.Tick();
    }

    public virtual void Spawn(Vector3 position)
    {
        _navMeshAgent.Warp(position);
        _spawnPosition = position;
    }

    public virtual void TakeDamage(float value)
    {
        Health -= value;
    }

    public Vector2 GetPosition()
    {
        return _rb.position;
    }
}
