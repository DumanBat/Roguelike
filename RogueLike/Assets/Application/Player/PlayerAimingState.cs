using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimingState : IState
{
    private readonly string HORIZONTAL = "Horizontal";
    private readonly string VERTICAL = "Vertical";
    private readonly string IS_RUNNING = "isRunning";
    private readonly string IS_AIMING = "isAiming";
    private readonly string AIM_HORIZONTAL = "AimHorizontal";
    private readonly string AIM_VERTICAL = "AimVertical";

    private Rigidbody2D _rb;
    private Animator _animator;
    private float _moveSpeed;
    private WeaponController _weaponController;
    private PlayerView _playerView;
    private Camera _cam;

    private Vector2 _movement;
    private bool _isMoving;
    private Vector3 _aimPos;

    public PlayerAimingState(Rigidbody2D rb, Animator animator, float moveSpeed, 
        WeaponController weaponController, PlayerView  playerView, Camera cam)
    {
        _rb = rb;
        _animator = animator;
        _moveSpeed = moveSpeed * 0.75f;
        _weaponController = weaponController;
        _playerView = playerView;
        _cam = cam;
    }

    public void Tick()
    {
        Move();
        Aim();

        if (_weaponController.GetCurrentWeapon().isAuto)
        {
            if (Input.GetMouseButton(0))
                Shot();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                Shot();
        }

        if (Input.GetKeyDown(KeyCode.Q))
            SwapWeapons();

        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    public void FixedTick()
    {
        if (!_isMoving) return;

        _rb.MovePosition(_rb.position + _movement * Time.fixedDeltaTime * _moveSpeed);
    }

    public void Move()
    {
        _movement = new Vector2(Input.GetAxis(HORIZONTAL), Input.GetAxis(VERTICAL));
        _isMoving = _movement != Vector2.zero ? true : false;

        _animator.SetBool(IS_RUNNING, _isMoving);
    }

    public void Aim()
    {
        Vector3 mousePos = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var rbPos = new Vector3(_rb.position.x, _rb.position.y, 0f);
        _aimPos = mousePos - rbPos;

        _animator.SetBool(IS_AIMING, true);
        _animator.SetFloat(AIM_HORIZONTAL, _aimPos.normalized.x);
        _animator.SetFloat(AIM_VERTICAL, _aimPos.normalized.y);
        var directionIndex = GetDirection(_aimPos);

        _aimPos = mousePos;

        _weaponController.SetActiveWeaponDirection(directionIndex);
        _playerView.SetActiveHand(directionIndex);
    }

    private void Shot() => _weaponController.Shot(_aimPos);
    private void Reload() => _weaponController.Reload();

    private void SwapWeapons()
    {
        _weaponController.DisableWeaponSprites();
        _weaponController.SwapWeapon();
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

    public void OnEnter()
    {

    }

    public void OnExit()
    {
        _weaponController.DisableWeaponSprites();
        _playerView.DisableHand();
        _animator.SetBool(IS_AIMING, false);
    }
}
