using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyBaseBehaviour : MonoBehaviour//, IEnemyStateSwitcher
{
    /*protected Enemy _enemy;
    protected EnemyBaseState _currentState;

    protected List<EnemyBaseState> _allStates;

    public virtual void Init(Enemy enemy)
    {
        _enemy = enemy;
    }

    public virtual void Attack()
    {
        _currentState.Attack();
    }

    public virtual void Move()
    {
        _currentState.Move();
    }

    public void Update()
    {
        _currentState.Update();
    }

    public void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    public virtual void SwitchState<T>() where T : EnemyBaseState
    {
        var state = _allStates.FirstOrDefault(s => s is T);
        _currentState.Exit();
        state.Enter();
        _currentState = state;
    }*/
}
