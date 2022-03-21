using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBasic : Item
{
    public override void Apply()
    {
        PlayerController.Instance.Health += _value;
    }
}
