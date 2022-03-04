using System.Collections;
using UnityEngine;

public interface IPushable
{
    IEnumerator GetPush(Vector2 pushDirection, float duration);
}
