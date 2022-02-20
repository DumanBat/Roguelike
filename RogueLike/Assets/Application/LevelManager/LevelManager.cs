using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private EnemyFactory enemyFactory;
    private RoomTemplates _roomTemplates;

    [SerializeField]
    private int _roomsAmount;
    public int GetRoomsAmount() => _roomsAmount;

    private float _lastSpawnedRoomTime = -9999f;
    private bool _roomSpawnStarted = false;
    private bool _roomSpawnCompleted = false;

    public Action OnRoomSpawned;
    // TODO: убрать инициализацию уровня из Start

    private void Awake()
    {
        _roomTemplates = GetComponent<RoomTemplates>();
        //OnRoomSpawned += NavMeshController.Instance.Init;
        Debug.Log("sh");
    }

    public void Start()
    {
        Init();
    }

    private void Update()
    {
        if (_roomSpawnCompleted) return;
        if (!_roomSpawnStarted) return;

        if (Time.time > _lastSpawnedRoomTime)
        {
            _roomSpawnCompleted = true;
            NavMeshController.Instance.Init();
            SpawnEnemies();

            foreach (var room in _roomTemplates.spawnedRooms)
                room.SpawnEnemies();
        }
    }

    public void Init()
    {
        var startingRoom = Instantiate(_roomTemplates.startingRooms[0], Vector3.zero, Quaternion.identity);
        startingRoom.Init();
        _roomSpawnStarted = true;
    }

    public void SpawnEnemies()
    {
        SpawnEnemy(EnemyType.GreenJelly, new Vector2(5.0f, 5.0f));
        SpawnEnemy(EnemyType.PinkJelly, new Vector2(2.0f, 5.0f));
        SpawnEnemy(EnemyType.PinkJelly, new Vector2(10.0f, 5.0f));

        /*SpawnEnemy(EnemyType.PinkJelly, new Vector2(4.0f, 5.0f));
        SpawnEnemy(EnemyType.PinkJelly, new Vector2(6.0f, 5.0f));
        SpawnEnemy(EnemyType.PinkJelly, new Vector2(8.0f, 5.0f));*/
    }
    public void SpawnEnemy(EnemyType type, Vector2 position)
    {
        var enemy = enemyFactory.Get(type);
        enemy.Spawn(position);
    }

    public RoomTemplates GetRoomTemplates() => _roomTemplates;

    public void SetLastSpawnedRoomTime(float time) => _lastSpawnedRoomTime = time + 0.5f;
}
