using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class PlayerController : MonoBehaviour
{
    private PlayerView _currentView;
    public WeaponController weaponController;

    public float _moveSpeed = 6f;
    public Animator animator;

    private Camera cam;

    private Vector2 _movement;
    private Rigidbody2D rb;
    private Vector3 _aimPos;

    private bool _isRunning;
    private bool _isAiming;

    private void Awake()
    {
        _currentView = GetComponent<PlayerView>();
        rb = GetComponent<Rigidbody2D>();
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

        if (Input.GetMouseButtonDown(0) && _isAiming)
            Shot();

        if (Input.GetKeyDown(KeyCode.Q))
            SwapWeapons();
    }

    private void FixedUpdate()
    {
        if (!_isRunning) return;

        var moveSpeed = _isAiming ? _moveSpeed * 0.75f : _moveSpeed;
        rb.MovePosition(rb.position + _movement * Time.fixedDeltaTime * moveSpeed);
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
        if (!_isAiming)
        {
            weaponController.DisableWeaponSprites();
            _currentView.DisableHand();
            animator.SetBool("isAiming", _isAiming);
            return;
        }

        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15.0f));
        var rbPos = new Vector3(rb.position.x, rb.position.y, 15.0f);
        _aimPos = mousePos - rbPos;

        animator.SetBool("isAiming", _isAiming);
        animator.SetFloat("AimHorizontal", _aimPos.normalized.x);
        animator.SetFloat("AimVertical", _aimPos.normalized.y);

        var directionIndex = GetDirection(_aimPos);
        weaponController.SetActiveWeaponSprite(directionIndex);
        _currentView.SetActiveHand(directionIndex);
    }

    private void Shot()
    {
        weaponController.currentWeapon.Shot(_aimPos);
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
}
