using UnityEngine;
using UnityEngine.AI;

public class SnakeAggroState : IState
{
    private static readonly int CHASE = Animator.StringToHash("isWalk");

    private Enemy _enemy;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private readonly EnemyDetector _enemyDetector;

    private float _shootingRange;
    private float _initialStoppingDistance;

    public SnakeAggroState(Enemy enemy, NavMeshAgent navMeshAgent, Animator animator, EnemyDetector enemyDetector, float shootingRange)
    {
        _enemy = enemy;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _enemyDetector = enemyDetector;
        _shootingRange = shootingRange;

        _initialStoppingDistance = navMeshAgent.stoppingDistance;
    }

    public void Tick()
    {
        var targetPosition = _enemyDetector.detectedTarget.GetPosition();
        var targetDistance = Vector3.Distance(_enemy.transform.position, targetPosition);

        if (targetDistance < _shootingRange)
        {
            _navMeshAgent.isStopped = true;
            _enemy.targetToShoot = _enemyDetector.detectedTarget;
        }
        else
            _navMeshAgent.SetDestination(targetPosition);
    }

    public void FixedTick() { }

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
