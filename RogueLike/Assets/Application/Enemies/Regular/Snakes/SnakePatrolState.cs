using UnityEngine;
using UnityEngine.AI;

public class SnakePatrolState : IState
{
    private readonly Enemy _enemy;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private readonly float _patrolRange;

    private bool _isPatrolPointSet;
    private Vector3 _currentPatrolPoint;

    private Vector3 _lastPosition = Vector3.zero;
    private float _timeStuck;

    public SnakePatrolState(Enemy enemy, NavMeshAgent navMeshAgent, Animator animator, float patrolRange)
    {
        _enemy = enemy;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _patrolRange = patrolRange;
    }

    public void Tick()
    {
        if (!_isPatrolPointSet)
        {
            _currentPatrolPoint = GetPatrolPoint();
            _isPatrolPointSet = true;
            _timeStuck = 0.0f;
            _navMeshAgent.SetDestination(_currentPatrolPoint);
        }

        var distance = Vector3.Distance(_enemy.transform.position, _lastPosition);

        if (distance <= 0.001f)
            _timeStuck += Time.deltaTime;

        if (distance < 2f && _timeStuck >= 1f)
            _isPatrolPointSet = false;

        _lastPosition = _enemy.transform.position;
    }

    public void FixedTick() { }

    private Vector3 GetPatrolPoint()
    {
        var posX = Random.Range(-_patrolRange, _patrolRange);
        var posY = Random.Range(-_patrolRange, _patrolRange);
        var destination = new Vector3(_enemy.transform.position.x + posX, _enemy.transform.position.y + posY, 0.0f);
        var path = new NavMeshPath();
        _navMeshAgent.CalculatePath(destination, path);

        return path.status == NavMeshPathStatus.PathComplete
            ? destination
            : GetPatrolPoint();
    }

    public void OnEnter()
    {
        _isPatrolPointSet = false;
        _timeStuck = 0f;
        _navMeshAgent.enabled = true;
        _navMeshAgent.speed = 0.5f;
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
    }
}
