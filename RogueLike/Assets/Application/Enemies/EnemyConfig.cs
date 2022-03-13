using System;
using UnityEngine;

[Serializable]
public class EnemyConfig
{
    public Enemy enemyPrefab;

    [Range(0, 50)]
    public int Health = 10;
    [Range(0, 10)]
    public int Damage = 5;
    [Range(0.5f, 10.0f)]
    public float Scale = 1.0f;
    [Range(0.0f, 20.0f)]
    public float PatrolRange = 5.0f;
    [Range(1.0f, 100.0f)]
    public float AggroRange = 5.0f;
    [Range(1.0f, 5.0f)]
    public float MeleeRange = 1.5f;
    [Range(0.5f, 5.0f)]
    public float AttackCooldown = 1f;
    [Range(0.5f, 10.0f)]
    public float AggroCooldown = 3f;
}
