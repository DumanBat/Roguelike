using System;
using UnityEngine;

[Serializable]
public class EnemyConfig
{
    public Enemy enemyPrefab;

    /*[FloatValueSlider(1f, 500f)]
    public FloatValue Health = new FloatValue(10f);
    [FloatValueSlider(0.5f, 2f)]
    public FloatValue Scale = new FloatValue(1f);
    [FloatValueSlider(0f, 20f)]
    public FloatValue PatrolRange = new FloatValue(5f);
    [FloatValueSlider(1f, 10f)]
    public FloatValue AggroRange = new FloatValue(5f);*/
    [Range(0.0f, 500.0f)]
    public float Health = 10.0f;
    [Range(0.5f, 2.0f)]
    public float Scale = 1.0f;
    [Range(0.0f, 20.0f)]
    public float PatrolRange = 5.0f;
    [Range(1.0f, 100.0f)]
    public float AggroRange = 5.0f;
}
