using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Enemy
{
    public override string enemyName => "snake";

    public override void Init()
    {
        var patrol = new SnakePatrolState(this, _navMeshAgent, _animator, PatrolRange);
        var aggro = new SnakeAggroState(this, _navMeshAgent, _animator, _enemyDetector, ShootingRange);
        var attack = new SnakeAttackState(this, _weaponController);

        At(patrol, aggro, () => _enemyDetector.EnemyInRange);
        At(aggro, patrol, () => _enemyDetector.EnemyInRange == false);
        At(aggro, attack, () => HasTargetToShoot() && _enemyDetector.EnemyInRange);
        At(attack, aggro, () => HasTargetToShoot() == false && _enemyDetector.EnemyInRange);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        _stateMachine.SetState(patrol);
    }
}
