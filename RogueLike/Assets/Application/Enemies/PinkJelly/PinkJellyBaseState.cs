using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PinkJellyBaseState : EnemyBaseState
{
    protected PinkJellyBaseState(Enemy enemy, IEnemyStateSwitcher stateSwitcher) : base(enemy, stateSwitcher)
    {
    }
}
