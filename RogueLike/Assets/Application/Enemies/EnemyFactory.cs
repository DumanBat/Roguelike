using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Factory", menuName = "Factories/Enemy")]

public class EnemyFactory: GameObjectFactory
{
    [Header("Bosses")]
    [SerializeField]
    private EnemyConfig _bossShark;
    private List<EnemyType> _bosses = new List<EnemyType>() 
    { 
        EnemyType.BossShark 
    };
    public List<EnemyType> GetBossesTypes() => _bosses;

    [Header("Enemies")]
    [SerializeField]
    private EnemyConfig _greenJelly;
    [SerializeField]
    private EnemyConfig _pinkJelly;
    private List<EnemyType> _enemies = new List<EnemyType>() 
    { 
        EnemyType.GreenJelly, 
        EnemyType.PinkJelly 
    };
    public List<EnemyType> GetEnemiesTypes() => _enemies;

    public Enemy Get(EnemyType type)
    {
        var config = GetConfig(type);
        Enemy instance = CreateGameObjectInstance(config.enemyPrefab);
        instance.OriginFactory = this;
        instance.Init(config.Health, config.Damage, config.Scale, config.PatrolRange, config.AggroRange, config.MeleeRange, config.AttackCooldown, config.AggroCooldown);
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
            case EnemyType.BossShark:
                return _bossShark;
        }
        Debug.LogError($"No config for: {type}");
        return _greenJelly;
    }
}
