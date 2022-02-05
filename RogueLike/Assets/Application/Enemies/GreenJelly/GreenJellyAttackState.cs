using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenJellyAttackState : GreenJellyBaseState
{
    public GreenJellyAttackState(Enemy enemy, IEnemyStateSwitcher stateSwitcher) : base(enemy, stateSwitcher)
    {
    }

    public override void Attack()
    {
        Debug.Log("green jelly ATTACK while attacking");
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void Move()
    {
        Debug.Log("green jelly MOVE while attacking");
    }

    public override void Update()
    {

    }
}
