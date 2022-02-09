using System;
using UnityEngine;

public class PinkJelly : Enemy
{
    public override string enemyName => "pink jelly";

    public override void Init(float health, float scale, float patrolRange, float aggroRange, float meleeRange)
    {
        base.Init(health, scale, patrolRange, aggroRange, meleeRange);

        var patrol = new JellyPatrolState(this, _navMeshAgent, _animator, PatrolRange);
        var attack = new JellyAttackState(this, _navMeshAgent, _animator, _enemyDetector);

        _stateMachine.AddAnyTransition(attack, () => _enemyDetector.EnemyInRange);
        At(attack, patrol, () => _enemyDetector.EnemyInRange == false);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        _stateMachine.SetState(patrol);
    }
}
