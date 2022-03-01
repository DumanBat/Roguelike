using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private Room _startingRoom;
    public Action OnRoomSpawned;
    // TODO: убрать инициализацию уровня из Start

    private void Awake()
    {
        _roomTemplates = GetComponent<RoomTemplates>();
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

            ConfigureRooms();
        }
    }

    public void Init()
    {
        _startingRoom = Instantiate(_roomTemplates.startingRooms[0], Vector3.zero, Quaternion.identity);
        _startingRoom.Init();

        _roomTemplates.spawnedRooms.Remove(_startingRoom);
        _roomSpawnStarted = true;
    }

    private void ConfigureRooms()
    {
        // only enemies config
        _startingRoom.OpenDoors();

        foreach (var room in _roomTemplates.spawnedRooms)
        {
            var enemiesToSpawn = ConfigureEnemies();

            room.SpawnEnemies(enemiesToSpawn);
        }
    }

    private List<EnemyType> ConfigureEnemies()
    {
        var enemiesAmountToSpawn = UnityEngine.Random.Range(2, 6);
        List<EnemyType> enemiesToSpawn = new List<EnemyType>();
        for (int i = 0; i < enemiesAmountToSpawn; i++)
        {
            var avaliableEnemyTypes = (int)Enum.GetValues(typeof(EnemyType)).Cast<EnemyType>().Max();
            var enemyType = (EnemyType)UnityEngine.Random.Range(0, avaliableEnemyTypes + 1);

            enemiesToSpawn.Add(enemyType);
        }

        return enemiesToSpawn;
    }

    public Enemy SpawnEnemy(EnemyType type, Vector2 position)
    {
        var enemy = enemyFactory.Get(type);
        enemy.Spawn(position);

        return enemy;
    }

    public RoomTemplates GetRoomTemplates() => _roomTemplates;

    public void SetLastSpawnedRoomTime(float time) => _lastSpawnedRoomTime = time + 0.5f;
}
