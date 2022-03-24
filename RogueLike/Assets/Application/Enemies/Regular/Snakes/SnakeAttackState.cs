using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAttackState : IState
{
    private readonly Enemy _enemy;
    private WeaponController _weaponController;

    private IDamageable _targetToShoot;
    private float _cooldown;
    private float _lastAttackedAt;
    private float _shootingRange;

    public SnakeAttackState(Enemy enemy, WeaponController weaponController)
    {
        _enemy = enemy;
        _shootingRange = enemy.ShootingRange;
        _weaponController = weaponController;
        _cooldown = enemy.AttackCooldown;
    }

    public void Tick()
    {
        var targetPositionVector2 = _targetToShoot.GetPosition();
        var targetPosition = new Vector3(targetPositionVector2.x, targetPositionVector2.y, 0.0f);

        var targetDistance = Vector3.Distance(_enemy.transform.position, targetPosition);

        if (targetDistance > _shootingRange)
        {
            _enemy.targetToShoot = null;
        }
        else
        {
            if (Time.time > _lastAttackedAt + _cooldown)
            {
                var directionIndex = _enemy.GetDirection(targetPosition - _enemy.transform.position);
                _weaponController.SetActiveWeaponDirection(directionIndex);
                _weaponController.Shot(targetPosition);
                _lastAttackedAt = Time.time;
            }
        }
    }

    public void FixedTick() { }

    public void OnEnter()
    {
        _targetToShoot = _enemy.targetToShoot;
    }

    public void OnExit() { }
}
