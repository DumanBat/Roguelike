public abstract class EnemyBaseState
{
    protected readonly Enemy _enemy;
    protected readonly IEnemyStateSwitcher _stateSwitcher;

    protected EnemyBaseState(Enemy enemy, IEnemyStateSwitcher stateSwitcher)
    {
        _enemy = enemy;
        _stateSwitcher = stateSwitcher;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void Move();

    public abstract void Attack();

    public abstract void Update();

    public abstract void FixedUpdate();
}
