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
    /// Remove health display from Update();
    /// 
    /// Rework reference to WeaponController in Update() Shot() section
    /// </summary>
    private PlayerView _currentView;
    public WeaponController weaponController;

    public float _moveSpeed = 3f;
    public Animator animator;

    private Camera cam;

    private Vector2 _movement;
    private Rigidbody2D _rb;
    public Vector2 GetPosition() => _rb.position;
    public void SetPosition(Vector2 position) => _rb.position = position;
    private Vector3 _aimPos;

    private bool _isActive;
    public bool IsActive() => _isActive;
    private bool _isRunning;
    private bool _isAiming;

    private int _maxHealth;
    public int GetMaxHealthValue() => _maxHealth;
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
    public Action<int> onHealthChange;
    public Action<int> onMaxHealthChange;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _currentView = GetComponent<PlayerView>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init(PlayerWalkthroughData playerData = null)
    {
        if (playerData != null)
        {
            Health = playerData.health;
            _maxHealth = playerData.maxHealth;
            weaponController.Init(playerData.weaponConfigs);
        }
        else
        {
            Health = 10;
            _maxHealth = 10;
        }

        onMaxHealthChange.Invoke(_maxHealth);
        onHealthChange.Invoke(Health);

        cam = GameManager.Instance.cameraController.GetCamera();
        _isActive = true;
    }

    private void Update()
    {
        if (!_isActive) return;

        Move();

        _isAiming = Input.GetMouseButton(1);
        Aim();

        if (weaponController.currentWeapon != null && _isAiming)
        {
            if (!weaponController.currentWeapon.isAuto)
            {
                if (Input.GetMouseButtonDown(0))
                    Shot();
            }
            else
            {
                if (Input.GetMouseButton(0))
                    Shot();
            }

        }

        if (Input.GetKeyDown(KeyCode.Q))
            SwapWeapons();

        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    private void FixedUpdate()
    {
        if (!_isActive) return;
        if (!_isRunning) return;

        var moveSpeed = _isAiming ? _moveSpeed * 0.75f : _moveSpeed;
        _rb.MovePosition(_rb.position + _movement * Time.fixedDeltaTime * moveSpeed);
    }

    private void Move()
    {
        _movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _isRunning = _movement != Vector2.zero ? true : false;

        animator.SetBool("isRunning", _isRunning);
        animator.SetFloat("MoveHorizontal", _movement.x);
        animator.SetFloat("MoveVertical", _movement.y);
        animator.SetFloat("Magnitude", _movement.magnitude);
    }

    private void Aim()
    {
        if (weaponController.currentWeapon == null)
            return;

        if (!_isAiming)
        {
            weaponController.DisableWeaponSprites();
            _currentView.DisableHand();
            animator.SetBool("isAiming", _isAiming);
            return;
        }

        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var rbPos = new Vector3(_rb.position.x, _rb.position.y, 0f);
        _aimPos = mousePos - rbPos;

        animator.SetBool("isAiming", _isAiming);
        animator.SetFloat("AimHorizontal", _aimPos.normalized.x);
        animator.SetFloat("AimVertical", _aimPos.normalized.y);
        var directionIndex = GetDirection(_aimPos);

        _aimPos = mousePos;

        weaponController.SetActiveWeaponDirection(directionIndex);
        _currentView.SetActiveHand(directionIndex);
    }

    private void Shot()
    {
        weaponController.Shot(_aimPos);
    }

    private void Reload()
    {
        weaponController.Reload();
    }

    private void SwapWeapons()
    {
        weaponController.DisableWeaponSprites();
        weaponController.SwapWeapon();
    }

    private int GetDirection(Vector3 position)
    {
        var directionX = position.normalized.x;
        var directionY = position.normalized.y;
        var currentDirection = Mathf.Abs(directionX) > Mathf.Abs(directionY) ? directionX : directionY;

        if (currentDirection == directionX)
            return currentDirection > 0 ? 1 : 3;
        else
            return currentDirection > 0 ? 0 : 2;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<IPickable>()?.PickUp();
    }

    public void TakeDamage(int value)
    {
        Health -= value;
        onHealthChange.Invoke(Health);
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
