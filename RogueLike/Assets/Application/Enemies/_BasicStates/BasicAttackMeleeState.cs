using UnityEngine;
using UnityEngine.AI;

public class BasicAttackMeleeState : IState
{
    private static readonly int CHASE = Animator.StringToHash("isWalk");
    private static readonly int MELEE_ATTACK = Animator.StringToHash("doMeleeAttack");

    private readonly Enemy _enemy;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private readonly EnemyDetector _enemyDetector;

    private float _cooldown;
    private float _lastAttackedAt;

    private float _initialStoppingDistance;
    public BasicAttackMeleeState(Enemy enemy, NavMeshAgent navMeshAgent, Animator animator, EnemyDetector enemyDetector)
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

        if (_enemyDetector.GetMeleeDetector().enemyInMeleeRange)
        {
            if (Time.time > _lastAttackedAt + _cooldown)
            {
                _enemyDetector.detectedTarget.TakeDamage(_enemy.Damage);
                _animator.SetTrigger(MELEE_ATTACK);
                _lastAttackedAt = Time.time;
            }
        }
    }
    public void FixedTick()
    {

    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.speed = 2.5f;
        _navMeshAgent.stoppingDistance = _enemy.MeleeRange;
        _animator.SetBool(CHASE, true);
    }

    public void OnExit()
    {
        _navMeshAgent.stoppingDistance = _initialStoppingDistance;
        _navMeshAgent.enabled = false;
        _animator.SetBool(CHASE, false);
    }
}
