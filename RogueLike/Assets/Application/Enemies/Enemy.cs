using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public abstract class Enemy: MonoBehaviour, IDamageable
{
    [SerializeField]
    private Transform _model;

    public abstract string enemyName { get; }
    public EnemyFactory OriginFactory { get; set; }
    public Room OriginRoom { get; set; }

    protected Rigidbody2D _rb;
    protected NavMeshAgent _navMeshAgent;
    [SerializeField]
    protected Animator _animator;
    protected StateMachine _stateMachine;
    [SerializeField]
    protected EnemyDetector _enemyDetector;

    protected Vector2 _spawnPosition;

    private int _health;
    public int Health
    {
        get
        {
            return _health;
        }
        private set
        {
            _health = value > 0 ? value : 0;
        }
    }
    public int Damage { get; private set; }
    public float Scale { get; private set; }
    public float PatrolRange { get; private set; }
    public float AggroRange { get; private set; }
    public float MeleeRange { get; private set; }
    public float AttackCooldown { get; private set; }
    public float AggroCooldown { get; private set; }

    public Action onEnemyDie;
    private static readonly int DeathHash = Animator.StringToHash("isDie");
    private static readonly int DeathStateHash = Animator.StringToHash("Base Layer.Die");


    public virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _stateMachine = new StateMachine();

        onEnemyDie += () => StartCoroutine(Die());
    }

    public virtual void Init(int health, int damage, float scale, float patrolRange, 
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

    public virtual void Spawn(Vector2 position)
    {
        _navMeshAgent.Warp(new Vector3(position.x, position.y, 0.0f));
        _spawnPosition = position;
    }

    public virtual void TakeDamage(int value)
    {
        Health -= value;
        if (_health <= 0)
        {
            onEnemyDie.Invoke();
        }
    }

    public virtual IEnumerator Die()
    {
        _animator.SetBool(DeathHash, true);

        while(_animator.GetCurrentAnimatorStateInfo(0).fullPathHash != DeathStateHash)
            yield return null;

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

        if (OriginRoom != null)
            OriginRoom.RemoveFromSpawnedEnemiesList(this);
        Destroy(this.gameObject);
    }

    public Vector2 GetPosition()
    {
        return _rb.position;
    }
}
