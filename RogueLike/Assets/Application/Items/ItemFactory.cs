using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Factory", menuName = "Factories/Item")]

public class ItemFactory : GameObjectFactory
{
    [Header("Tier 3")]
    [SerializeField]
    private ItemConfig _heart;
    [SerializeField]
    private ItemConfig _ammo;

    public Item Get(ItemType type)
    {
        var config = GetConfig(type);
        Item instance = CreateGameObjectInstance(config.ItemPrefab);
        instance.OriginFactory = this;
        instance.Init(config);
        return instance;
    }

    public Item Get(ItemConfig config)
    {
        Item instance = CreateGameObjectInstance(config.ItemPrefab);
        instance.OriginFactory = this;
        instance.Init(config);
        return instance;
    }

    private ItemConfig GetConfig(ItemType type)
    {
        switch (type)
        {
            case ItemType.Heart:
                return _heart;
            case ItemType.Ammo:
                return _ammo;
        }
        Debug.LogError($"No config for: {type}");
        return _heart;
    }
}
