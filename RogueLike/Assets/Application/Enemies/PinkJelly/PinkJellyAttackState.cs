using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkJellyAttackState : PinkJellyBaseState
{
    public PinkJellyAttackState(Enemy enemy, IEnemyStateSwitcher stateSwitcher) : base(enemy, stateSwitcher)
    {
    }

    public override void Attack()
    {
        Debug.Log("Pink jelly ATTACK while attacking");
    }

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void FixedUpdate()
    {

    }

    public override void Move()
    {
        Debug.Log("Pink jelly MOVE while attacking");
    }

    public override void Update()
    {

    }
}
