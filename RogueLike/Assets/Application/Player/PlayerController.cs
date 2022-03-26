using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Modules.Core;

public class PlayerController : Singleton<PlayerController>, IDamageable, IPushable
{
    /// <summary>
    /// TODO: 
    /// Rework reference to WeaponController in Update() Shot() section
    /// </summary>
    public WeaponController weaponController;
    public Animator animator;

    private PlayerView _currentView;
    private Camera cam;
    private Rigidbody2D _rb;
    
    private float _moveSpeed = 3.5f;
    private float _slideDuration = 0.7f;

    private bool _isActive;
    public bool IsSliding { get; set; }

    private int _maxHealth;
    private int _health;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
            onMaxHealthChange.Invoke(_maxHealth);
        }
    }
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            if (value < 0)
                _health = 0;
            else
                _health = value > _maxHealth ? _maxHealth : value;

            onHealthChange.Invoke(_health);
        }
    }
    public int GetMaxHealthValue() => _maxHealth;
    public Action<int> onHealthChange;
    public Action<int> onMaxHealthChange;

    private StateMachine _stateMachine;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _currentView = GetComponent<PlayerView>();
        _rb = GetComponent<Rigidbody2D>();
        _stateMachine = new StateMachine();
    }

    public void Init(PlayerWalkthroughData playerData = null)
    {
        if (playerData != null)
        {
            MaxHealth = playerData.maxHealth;
            Health = playerData.health;
            weaponController.Init(playerData.weaponConfigs);
        }
        else
        {
            MaxHealth = 6;
            Health = 6;
        }

        cam = GameManager.Instance.cameraController.GetCamera();

        var walking = new PlayerWalkingState(this, _rb, animator, _moveSpeed, weaponController, _slideDuration);
        var aiming = new PlayerAimingState(_rb, animator, _moveSpeed, weaponController, _currentView, cam);

        At(walking, aiming, () => IsAiming());
        At(aiming, walking, () => !IsAiming());
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        _stateMachine.SetState(walking);
        _isActive = true;
    }

    private void Update()
    {
        if (!_isActive) return;

        _stateMachine.Tick();
    }

    private void FixedUpdate()
    {
        if (!_isActive) return;

        _stateMachine.FixedTick();
    }

    public bool IsAiming()
    {
        return
            weaponController.GetCurrentWeapon() != null
            && Input.GetMouseButton(1);
    }

    private bool HasDamageResist()
    {
         return IsSliding;
    }

    public Vector2 GetPosition() => _rb.position;
    public void SetPosition(Vector2 position) => _rb.position = position;
    public void TakeDamage(int value)
    {
        if (HasDamageResist()) return;
        
        Health -= value;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<IPickable>()?.PickUp();
    }

    public IEnumerator GetPush(Vector2 pushDirection, float duration)
    {
        _rb.AddForce(pushDirection, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        yield return new WaitForEndOfFrame();
        _rb.velocity = Vector2.zero;
    }

    public void Unload()
    {
        weaponController.Unload();
    }
}
