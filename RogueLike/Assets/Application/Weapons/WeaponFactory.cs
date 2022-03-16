using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Factory", menuName = "Factories/Weapon")]

public class WeaponFactory : GameObjectFactory
{
    [Header("Tier 1")]
    [SerializeField]
    private WeaponConfig _autoRifle;
    private List<WeaponType> _tierOne = new List<WeaponType>()
    {
        WeaponType.AutoRifle
    };
    public List<WeaponType> GetWeaponsTierOne() => _tierOne;

    [Header("Starting Weapons")]
    [SerializeField]
    private WeaponConfig _sidearm;
    private List<WeaponType> _startingWeapons = new List<WeaponType>()
    {
        WeaponType.Sidearm
    };
    public List<WeaponType> GetStartingWeapons() => _startingWeapons;

    public Weapon Get(WeaponType type)
    {
        var config = GetConfig(type);
        Weapon instance = CreateGameObjectInstance(config.weaponPrefab);
        instance.OriginFactory = this;
        instance.Init(config);
        return instance;
    }

    public Weapon Get(WeaponConfig config)
    {
        Weapon instance = CreateGameObjectInstance(config.weaponPrefab);
        instance.OriginFactory = this;
        instance.Init(config);
        return instance;
    }

    private WeaponConfig GetConfig(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Sidearm:
                return _sidearm;
            case WeaponType.AutoRifle:
                return _autoRifle;
        }
        Debug.LogError($"No config for: {type}");
        return _sidearm;
    }
}
