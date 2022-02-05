using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private EnemyFactory enemyFactory;

    // TODO: убрать инициализацию уровня из Start
    public void Start()
    {
        Init();
    }

    public void Init()
    {
        SpawnEnemy(EnemyType.GreenJelly, new Vector3(5.0f, 5.0f, 0.0f));
        SpawnEnemy(EnemyType.PinkJelly, new Vector3(2.0f, 5.0f, 0.0f));
        SpawnEnemy(EnemyType.PinkJelly, new Vector3(10.0f, 5.0f, 0.0f));
    }

    public void SpawnEnemy(EnemyType type, Vector3 position)
    {
        var enemy = enemyFactory.Get(type);
        enemy.Spawn(position);
    }
}
