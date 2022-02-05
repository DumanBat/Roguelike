public interface IEnemyStateSwitcher
{
    void SwitchState<T>() where T: EnemyBaseState;
}
