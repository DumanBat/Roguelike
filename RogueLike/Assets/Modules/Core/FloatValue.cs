using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct FloatValue
{
    [SerializeField]
    private float _value;

    public FloatValue(float value)
    {
        _value = value;
    }

    public float GetValue
    {
        get => _value;
    }
}

public class FloatValueSliderAttribute : PropertyAttribute
{
    public float Min { get; private set; }
    public float Max { get; private set; }

    public FloatValueSliderAttribute(float min, float max)
    {
        Min = min;
        Max = max < min ? min : max;
    }
}
