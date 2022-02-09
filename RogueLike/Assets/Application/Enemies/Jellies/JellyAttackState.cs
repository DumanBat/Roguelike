using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JellyAttackState : IState
{
    private readonly Enemy _enemy;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private readonly EnemyDetector _enemyDetector;

    private static readonly int AttackHash = Animator.StringToHash("isWalk");
    public JellyAttackState(Enemy enemy, NavMeshAgent navMeshAgent, Animator animator, EnemyDetector enemyDetector)
    {
        _enemy = enemy;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _enemyDetector = enemyDetector;
    }

    public void Tick()
    {
        Debug.Log("Attack state");

        _enemyDetector.detectedTarget.TakeDamage(5f);
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.speed = 2.5f;
        _animator.SetBool(AttackHash, true);
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetBool(AttackHash, false);
    }

    public IEnumerator AttackWithDelay()
    {
        yield return new WaitForSeconds(1f);
    }
}
