using System;
using UnityEngine;
public abstract class GreenJellyBaseState : EnemyBaseState
{
    protected GreenJellyBaseState(Enemy enemy, IEnemyStateSwitcher stateSwitcher) : base(enemy, stateSwitcher)
    {
    }
}
