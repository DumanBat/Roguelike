using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackRangeState : IState
{
    private readonly Enemy _enemy;
    private WeaponController _weaponController;

    private Weapon _currentWeapon;
    private IDamageable _targetToShoot;
    private float _cooldown;
    private float _lastAttackedAt;
    private float _shootingRange;

    public BasicAttackRangeState(Enemy enemy, WeaponController weaponController)
    {
        _enemy = enemy;
        _shootingRange = enemy.ShootingRange;
        _weaponController = weaponController;
        _cooldown = enemy.AttackCooldown;
    }

    public void Tick()
    {
        if (_currentWeapon.IsReloading())
            return;

        var targetPositionVector2 = _targetToShoot.GetPosition();
        var targetPosition = new Vector3(targetPositionVector2.x, targetPositionVector2.y, 0.0f);
        var targetDistance = Vector3.Distance(_enemy.transform.position, targetPosition);

        var directionIndex = _enemy.GetDirection(targetPosition - _enemy.transform.position);
        _weaponController.SetActiveWeaponDirection(directionIndex);

        if (targetDistance > _shootingRange)
        {
            _enemy.targetToShoot = null;
        }
        else
        {
            if (Time.time > _lastAttackedAt + _cooldown)
            {

                if (!_weaponController.Shot(targetPosition))
                    _weaponController.Reload();
                else
                    _lastAttackedAt = Time.time;
            }
        }
    }

    public void FixedTick() { }

    public void OnEnter()
    {
        _targetToShoot = _enemy.targetToShoot;
        _currentWeapon = _weaponController.GetCurrentWeapon();
    }

    public void OnExit() { }
}
