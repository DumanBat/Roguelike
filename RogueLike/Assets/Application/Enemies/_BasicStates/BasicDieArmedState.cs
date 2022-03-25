using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDieArmedState : IState
{
    private static readonly int DeathHash = Animator.StringToHash("isDie");
    private static readonly int DeathStateHash = Animator.StringToHash("Base Layer.Die");

    private Enemy _enemy;
    private Animator _animator;

    private WeaponController _weaponController;
    public BasicDieArmedState(Enemy enemy, Animator animator, WeaponController weaponController)
    {
        _enemy = enemy;
        _animator = animator;
        _weaponController = weaponController;
    }

    public virtual IEnumerator Die()
    {
        _animator.SetBool(DeathHash, true);
        _weaponController.Unload();

        while (_animator.GetCurrentAnimatorStateInfo(0).fullPathHash != DeathStateHash)
            yield return null;

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

        if (_enemy.OriginRoom != null)
            _enemy.OriginRoom.RemoveFromSpawnedEnemiesList(_enemy);

        _enemy.SelfDestroy();
    }

    public void FixedTick() { }
    public void OnEnter()
    {
        _enemy.StartCoroutine(Die());
    }
    public void OnExit() { }
    public void Tick() { }
}
