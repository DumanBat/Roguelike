using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private EnemyFactory enemyFactory;
    private RoomTemplates _roomTemplates;

    // TODO: убрать инициализацию уровня из Start

    private void Awake()
    {
        _roomTemplates = GetComponent<RoomTemplates>();
    }

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        var startingRoom = Instantiate(_roomTemplates.startingRooms[0], Vector3.zero, Quaternion.identity);
        startingRoom.Init();

        SpawnEnemy(EnemyType.GreenJelly, new Vector3(5.0f, 5.0f));
        SpawnEnemy(EnemyType.PinkJelly, new Vector3(2.0f, 5.0f));
        SpawnEnemy(EnemyType.PinkJelly, new Vector3(10.0f, 5.0f));

        SpawnEnemy(EnemyType.PinkJelly, new Vector3(4.0f, 5.0f));
        SpawnEnemy(EnemyType.PinkJelly, new Vector3(6.0f, 5.0f));
        SpawnEnemy(EnemyType.PinkJelly, new Vector2(8.0f, 5.0f));
    }

    public void SpawnEnemy(EnemyType type, Vector2 position)
    {
        var enemy = enemyFactory.Get(type);
        enemy.Spawn(position);
    }

    public RoomTemplates GetRoomTemplates() => _roomTemplates;
}
