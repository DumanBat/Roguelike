using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Modules.Core;

public class PlayerController : Singleton<PlayerController>, IDamageable
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
    private Vector3 _aimPos;

    private bool _isRunning;
    private bool _isAiming;

    private float _health = 100f;
    public float Health
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

    public TextMeshPro healthDisplay;
    private void Awake()
    {
        _currentView = GetComponent<PlayerView>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        Init();    
    }

    public void Init()
    {
        cam = GameManager.Instance.cameraController.GetCamera();
    }

    private void Update()
    {
        Move();

        _isAiming = Input.GetMouseButton(1);
        Aim();

        if (!weaponController.currentWeapon.isAuto)
        {
            if (Input.GetMouseButtonDown(0) && _isAiming)
                Shot();
        }
        else
        {
            if (Input.GetMouseButton(0) && _isAiming)
                Shot();
        }

        if (Input.GetKeyDown(KeyCode.Q))
            SwapWeapons();

        if (Input.GetKeyDown(KeyCode.R))
            Reload();

        healthDisplay.text = _health.ToString();
    }

    private void FixedUpdate()
    {
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

        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15.0f));
        var rbPos = new Vector3(_rb.position.x, _rb.position.y, 15.0f);
        _aimPos = mousePos - rbPos;

        animator.SetBool("isAiming", _isAiming);
        animator.SetFloat("AimHorizontal", _aimPos.normalized.x);
        animator.SetFloat("AimVertical", _aimPos.normalized.y);

        var directionIndex = GetDirection(_aimPos);
        weaponController.SetActiveWeaponDirection(directionIndex);
        _currentView.SetActiveHand(directionIndex);
    }

    private void Shot()
    {
        weaponController.currentWeapon.Shot(_aimPos);
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

    public void TakeDamage(float value)
    {
        Health -= value;
    }

    public Vector2 GetPosition()
    {
        return _rb.position;
    }
}
