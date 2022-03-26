using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDieState : IState
{
    private static readonly int DeathHash = Animator.StringToHash("isDie");
    private static readonly int DeathStateHash = Animator.StringToHash("Base Layer.Die");

    private Enemy _enemy;
    private Animator _animator;

    public BasicDieState(Enemy enemy, Animator animator)
    {
        _enemy = enemy;
        _animator = animator;
    }

    public virtual IEnumerator Die()
    {
        _animator.SetBool(DeathHash, true);

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
