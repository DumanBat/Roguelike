using System;

public class Shark : Enemy
{
    public override string enemyName => "boss_shark";

    public override void Init()
    {
        var patrol = new BasicPatrolState(this, _navMeshAgent, _animator, PatrolRange);
        var attack = new BasicAttackMeleeState(this, _navMeshAgent, _animator, _enemyDetector);
        var die = new BasicDieState(this, _animator);

        At(patrol, attack, () => _enemyDetector.EnemyInRange);
        At(attack, patrol, () => _enemyDetector.EnemyInRange == false);

        _stateMachine.AddAnyTransition(die, () => Health <= 0);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        _stateMachine.SetState(patrol);
    }
}
