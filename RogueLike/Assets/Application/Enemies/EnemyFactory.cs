using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Factory", menuName = "Factories/Enemy")]

public class EnemyFactory: GameObjectFactory
{
    [Header("Bosses")]
    [SerializeField]
    private EnemyConfig _bossShark;

    [Header("Enemies")]
    [SerializeField]
    private EnemyConfig _greenJelly;
    [SerializeField]
    private EnemyConfig _pinkJelly;
    [SerializeField]
    private EnemyConfig _snake;
    [SerializeField]
    private EnemyConfig _autoSnake;

    public Enemy Get(EnemyType type)
    {
        var config = GetConfig(type);
        Enemy instance = CreateGameObjectInstance(config.enemyPrefab);
        instance.OriginFactory = this;
        instance.Init(config);
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
            case EnemyType.Snake:
                return _snake;
            case EnemyType.AutoSnake:
                return _autoSnake;
            case EnemyType.BossShark:
                return _bossShark;
        }
        Debug.LogError($"No config for: {type}");
        return _greenJelly;
    }
}
