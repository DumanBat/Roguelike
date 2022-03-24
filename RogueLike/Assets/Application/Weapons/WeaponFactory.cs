using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Factory", menuName = "Factories/Weapon")]

public class WeaponFactory : GameObjectFactory
{
    [Header("Tier 1")]
    [SerializeField]
    private WeaponConfig _autoRifle;

    [Header("Starting Weapons")]
    [SerializeField]
    private WeaponConfig _sidearm;
    [SerializeField]
    private WeaponConfig _none;

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
            case WeaponType.None:
                return _none;
            case WeaponType.Sidearm:
                return _sidearm;
            case WeaponType.AutoRifle:
                return _autoRifle;
        }
        Debug.LogError($"No config for: {type}");
        return _sidearm;
    }
}
