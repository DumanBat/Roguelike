using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Factory", menuName = "Factories/Enemy")]

public class EnemyFactory: GameObjectFactory
{
    [SerializeField]
    private EnemyConfig _greenJelly, _pinkJelly;

    public Enemy Get(EnemyType type)
    {
        var config = GetConfig(type);
        Enemy instance = CreateGameObjectInstance(config.enemyPrefab);
        instance.OriginFactory = this;
        instance.Init(config.Health, config.Scale, config.PatrolRange, config.AggroRange);
        return instance;
    }

    private EnemyConfig GetConfig(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.GreenJelly:
                return _greenJelly;
            case EnemyType.PinkJelly:
                return _pinkJelly;
        }
        Debug.LogError($"No config for: {type}");
        return _greenJelly;
    }
}
