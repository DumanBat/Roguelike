using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectFactory: ScriptableObject
{
    protected T CreateGameObjectInstance<T>(T prefab) where T : MonoBehaviour
    {
        T instance = Instantiate(prefab);
        return instance;
    }
}
