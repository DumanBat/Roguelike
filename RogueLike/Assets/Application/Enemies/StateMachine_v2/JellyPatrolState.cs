using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JellyPatrolState : IState
{
    private readonly Enemy _enemy;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly float _patrolRange;

    private bool _isPatroling;
    private bool _isPatrolPointSet;
    private Vector3 _currentPatrolPoint;

    private Vector3 _lastPosition = Vector3.zero;
    private float _timeStuck;

    public JellyPatrolState(Enemy enemy, NavMeshAgent navMeshAgent, float patrolRange)
    {
        _enemy = enemy;
        _navMeshAgent = navMeshAgent;
        _patrolRange = patrolRange;

        _isPatrolPointSet = false;
        _isPatroling = false;
    }

    public void Tick()
    {
        if (!_isPatrolPointSet)
            _currentPatrolPoint = GetPatrolPoint();

        if (!_isPatroling)
            _navMeshAgent.SetDestination(_currentPatrolPoint);

        var distance = _enemy.transform.position - _currentPatrolPoint;

        if (distance.magnitude < 1f || _timeStuck >= 1f)
        {
            _isPatrolPointSet = false;
            _isPatroling = false;
        }

        if (Vector3.Distance(_enemy.transform.position, _lastPosition) <= 0f)
            _timeStuck += Time.deltaTime;

        _lastPosition = _enemy.transform.position;
    }

    private Vector3 GetPatrolPoint()
    {
        var posX = Random.Range(-_patrolRange, _patrolRange);
        var posY = Random.Range(-_patrolRange, _patrolRange);
        var destination = new Vector3(_enemy.transform.position.x + posX, _enemy.transform.position.y + posY, 0.0f);
        var path = new NavMeshPath();
        _navMeshAgent.CalculatePath(destination, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            _isPatrolPointSet = true;
            return destination;
        }
        else
        {
            return GetPatrolPoint();
        }
            
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}
