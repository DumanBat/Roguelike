using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Modules.Core;

public abstract class Enemy: MonoBehaviour, IDamageable
{
    private static readonly int DeathHash = Animator.StringToHash("isDie");
    private static readonly int DeathStateHash = Animator.StringToHash("Base Layer.Die");

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
    [SerializeField]
    protected WeaponController _weaponController;
    protected WeaponType _weaponType;

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
    public float ShootingRange { get; private set; }
    public float AttackCooldown { get; private set; }
    public float AggroCooldown { get; private set; }

    public Action onEnemyDie;
    public IDamageable targetToShoot { get; set; }
    public bool HasTargetToShoot() => targetToShoot != null;

    public virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _stateMachine = new StateMachine();

        onEnemyDie += () => StartCoroutine(Die());
    }

    public virtual void Init(EnemyConfig config)
    {
        _weaponType = config.WeaponType;
        if (_weaponType != WeaponType.None)
        {
            var weapon = GameManager.Instance.levelManager.GetLevelConfigurator().GetLootManager().SpawnWeapon(_weaponType, Vector3.zero);
            _weaponController.AddWeaponToInventory(weapon, true);
        }

        Health = config.Health;
        Damage = config.Damage;
        Scale = config.Scale;
        PatrolRange = config.PatrolRange;
        AggroRange = config.AggroRange;
        MeleeRange = config.MeleeRange;
        ShootingRange = config.ShootingRange;
        AttackCooldown = config.AttackCooldown;
        AggroCooldown = config.AggroCooldown;

        _enemyDetector.Init(AggroRange, MeleeRange, AggroCooldown);
        _model.localScale = new Vector3(Scale, Scale, Scale);

        Init();
    }

    public virtual void Init()
    {

    }

    public void Update()
    {
        _stateMachine.Tick();
    }

    public void FixedUpdate()
    {
        _stateMachine.FixedTick();
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

    public int GetDirection(Vector3 position)
    {
        var directionX = position.normalized.x;
        var directionY = position.normalized.y;
        var currentDirection = Mathf.Abs(directionX) > Mathf.Abs(directionY) ? directionX : directionY;

        if (currentDirection == directionX)
            return currentDirection > 0 ? 1 : 3;
        else
            return currentDirection > 0 ? 0 : 2;
    }
}
