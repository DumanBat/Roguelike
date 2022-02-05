using System.Collections.Generic;

public class GreenJellyBehaviour : EnemyBaseBehaviour, IEnemyStateSwitcher
{
    public override void Init(Enemy enemy)
    {
        base.Init(enemy);

        _allStates = new List<EnemyBaseState>()
        {
            new GreenJellyIdleState(_enemy, this),
            new GreenJellyAttackState(_enemy, this)
        };

        _currentState = _allStates[0];
    }
}
