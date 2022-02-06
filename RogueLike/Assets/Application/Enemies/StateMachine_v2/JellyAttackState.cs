using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JellyAttackState : IState
{
    private readonly Enemy _enemy;
    private readonly NavMeshAgent _navMeshAgent;

    public JellyAttackState(Enemy enemy, NavMeshAgent navMeshAgent)
    {
        _enemy = enemy;
        _navMeshAgent = navMeshAgent;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

}
