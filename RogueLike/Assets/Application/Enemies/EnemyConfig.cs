using System;
using UnityEngine;

[Serializable]
public class EnemyConfig
{
    public Enemy enemyPrefab;

    [Range(0.0f, 500.0f)]
    public float Health = 10.0f;
    [Range(0.5f, 2.0f)]
    public float Scale = 1.0f;
    [Range(0.0f, 20.0f)]
    public float PatrolRange = 5.0f;
    [Range(1.0f, 100.0f)]
    public float AggroRange = 5.0f;
}
