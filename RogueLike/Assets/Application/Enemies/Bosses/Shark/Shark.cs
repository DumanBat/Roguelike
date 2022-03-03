using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Enemy
{
    public override string enemyName => "boss_shark";

    public override void Init()
    {
        var patrol = new JellyPatrolState(this, _navMeshAgent, _animator, PatrolRange);
        var attack = new JellyAttackState(this, _navMeshAgent, _animator, _enemyDetector);

        _stateMachine.AddAnyTransition(attack, () => _enemyDetector.EnemyInRange);
        At(attack, patrol, () => _enemyDetector.EnemyInRange == false);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        _stateMachine.SetState(patrol);
    }
}
