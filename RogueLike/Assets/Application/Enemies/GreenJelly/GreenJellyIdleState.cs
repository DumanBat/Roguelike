using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenJellyIdleState : GreenJellyBaseState
{
    public GreenJellyIdleState(Enemy enemy, IEnemyStateSwitcher stateSwitcher) : base(enemy, stateSwitcher)
    {
    }

    public override void Attack()
    {
        Debug.Log("green jelly ATTACK while idle");
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
        _enemy.rb.MovePosition(_enemy.spawnPosition + new Vector2(0.0f, 3.0f) * Time.fixedDeltaTime * 0.05f);
    }

    public override void Move()
    {
        Debug.Log(_enemy.gameObject.name + " MOVE while idle");

    }

    public override void Update()
    {

    }
}
