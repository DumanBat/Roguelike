using System;
using UnityEngine;

public class PinkJelly : Enemy
{
    public override string enemyName => "pink jelly";

    public override void Init(float health, float scale, float patrolRange, float aggroRange)
    {
        base.Init(health, scale, patrolRange, aggroRange);

        var patrol = new JellyPatrolState(this, _navMeshAgent, PatrolRange);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        _stateMachine.SetState(patrol);
    }
}
