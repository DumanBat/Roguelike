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

    private float _cooldown;
    private float _lastAttackedAt;
    private static readonly int AttackHash = Animator.StringToHash("isWalk");

    private float _initialStoppingDistance;
    public JellyAttackState(Enemy enemy, NavMeshAgent navMeshAgent, Animator animator, EnemyDetector enemyDetector)
    {
        _enemy = enemy;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _enemyDetector = enemyDetector;
        _cooldown = enemy.AttackCooldown;
        _initialStoppingDistance = navMeshAgent.stoppingDistance;
    }

    public void Tick()
    {
        _navMeshAgent.SetDestination(_enemyDetector.detectedTarget.GetPosition());

        if (_enemyDetector._meleeDetector.enemyInMeleeRange)
        {
            if (Time.time > _lastAttackedAt + _cooldown)
            {
                _enemyDetector.detectedTarget.TakeDamage(_enemy.Damage);
                _animator.SetTrigger("doTouch");
                _lastAttackedAt = Time.time;
            }
        }
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.speed = 2.5f;
        _navMeshAgent.stoppingDistance = _enemy.MeleeRange;
        _animator.SetBool(AttackHash, true);
    }

    public void OnExit()
    {
        _navMeshAgent.stoppingDistance = _initialStoppingDistance;
        _navMeshAgent.enabled = false;
        _animator.SetBool(AttackHash, false);
    }
}
