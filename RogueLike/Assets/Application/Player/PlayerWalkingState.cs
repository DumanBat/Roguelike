using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingState : IState
{
    private readonly string HORIZONTAL = "Horizontal";
    private readonly string VERTICAL = "Vertical";
    private readonly string IS_RUNNING = "isRunning";
    private readonly string MOVE_HORIZONTAL = "MoveHorizontal";
    private readonly string MOVE_VERTICAL = "MoveVertical";
    private readonly string MAGNITUDE = "Magnitude";

    private Rigidbody2D _rb;
    private Animator _animator;
    private float _moveSpeed;
    private WeaponController _weaponController;

    private Vector2 _movement;
    private bool _isRunning;

    public PlayerWalkingState(Rigidbody2D rb, Animator animator, float moveSpeed, WeaponController weaponController)
    {
        _rb = rb;
        _animator = animator;
        _moveSpeed = moveSpeed;
        _weaponController = weaponController;
    }

    public void Tick()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Q))
            SwapWeapons();

        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    public void FixedTick()
    {
        if (!_isRunning) return;

        _rb.MovePosition(_rb.position + _movement * Time.fixedDeltaTime * _moveSpeed);
    }

    public void Move()
    {
        _movement = new Vector2(Input.GetAxis(HORIZONTAL), Input.GetAxis(VERTICAL));
        _isRunning = _movement != Vector2.zero ? true : false;

        _animator.SetBool(IS_RUNNING, _isRunning);
        _animator.SetFloat(MOVE_HORIZONTAL, _movement.x);
        _animator.SetFloat(MOVE_VERTICAL, _movement.y);
        _animator.SetFloat(MAGNITUDE, _movement.magnitude);
    }
    private void Reload() => _weaponController.Reload();

    private void SwapWeapons()
    {
        _weaponController.DisableWeaponSprites();
        _weaponController.SwapWeapon();
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }
}
