using System;

public class Snake : Enemy
{
    public override string enemyName => "snake";

    public override void Init()
    {
        var patrol = new BasicPatrolState(this, _navMeshAgent, _animator, PatrolRange);
        var aggro = new BasicAggroRangeState(this, _navMeshAgent, _animator, _enemyDetector, ShootingRange);
        var attack = new BasicAttackRangeState(this, _weaponController);
        var die = new BasicDieArmedState(this, _animator, _weaponController);

        At(patrol, aggro, () => _enemyDetector.EnemyInRange);
        At(aggro, patrol, () => _enemyDetector.EnemyInRange == false);
        At(aggro, attack, () => _enemyDetector.EnemyInRange && HasTargetToShoot());
        At(attack, aggro, () => _enemyDetector.EnemyInRange && HasTargetToShoot() == false);

        _stateMachine.AddAnyTransition(die, () => Health <= 0);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        _stateMachine.SetState(patrol);
    }
}
