using System;
using UnityEngine;

[Serializable]
public class ItemConfig
{
    public Item ItemPrefab;

    public bool IsEffect;
    public bool IsActivatable;
    public int Value;
}
