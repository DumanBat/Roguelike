using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Modules.Core;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private EnemyFactory enemyFactory;
    private LootManager _lootManager;
    private RoomTemplates _roomTemplates;
    public RoomTemplates GetRoomTemplates() => _roomTemplates;

    private float _lastSpawnedRoomTime = -9999f;
    public void SetLastSpawnedRoomTime(float time) => _lastSpawnedRoomTime = time + 0.5f;
    private bool _roomSpawnStarted = false;
    private bool _roomSpawnCompleted = false;

    private Room _startingRoom;
    public Action OnRoomSpawned;

    // CONFIG
    [SerializeField]
    private int _roomsAmount;
    public int GetRoomsAmount() => _roomsAmount;
    private int _lootRoomsAmount;
    // TODO: убрать инициализацию уровня из Start

    private void Awake()
    {
        _roomTemplates = GetComponent<RoomTemplates>();
        _lootManager = GetComponent<LootManager>();
    }

    public void Start()
    {
        Init(true);
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

    public void Init(bool isRandom, int roomsAmount = 0, int lootRoomsAmount = 0)
    {
        if (isRandom)
            RandomizeLevelConfig();
        else
        {
            _roomsAmount = roomsAmount;
            _lootRoomsAmount = lootRoomsAmount;
        }

        _startingRoom = Instantiate(_roomTemplates.startingRooms[0], Vector3.zero, Quaternion.identity);
        _startingRoom.Init();
        _roomTemplates.spawnedRooms.Remove(_startingRoom);

        _lootManager.SpawnWeapon(_lootManager.weaponToSpawn, new Vector3(3f, 0f, 0f));

        _roomSpawnStarted = true;
    }

    public void RandomizeLevelConfig()
    {
        _roomsAmount = UnityEngine.Random.Range(10, 16);
        _lootRoomsAmount = UnityEngine.Random.Range(1, 3);
    }

    private void ConfigureRooms()
    {
        var spawnedRooms = _roomTemplates.spawnedRooms;

        for (int i = 0; i < _lootRoomsAmount; i++)
        {
            var lootRoomIndex = GetRandomIndexOfRoomType(RoomTemplates.RoomType.LootRoom, spawnedRooms);
            spawnedRooms[lootRoomIndex].roomType = RoomTemplates.RoomType.LootRoom;
        }

        for (int i = 0; i < spawnedRooms.Count; i++)
        {
            switch (spawnedRooms[i].roomType)
            {
                case RoomTemplates.RoomType.LootRoom:
                    _lootManager.SpawnLootInRoom(spawnedRooms[i]);
                    break;
                case RoomTemplates.RoomType.EnemyRoom:
                    var enemiesToSpawn = ConfigureEnemies();
                    spawnedRooms[i].SpawnEnemies(enemiesToSpawn);
                    break;
            }
        }

        _startingRoom.OpenDoors();
    }

    private int GetRandomIndexOfRoomType(RoomTemplates.RoomType roomType, List<Room> spawnedRooms)
    {
        var index = UnityEngine.Random.Range(0, spawnedRooms.Count);
        return spawnedRooms[index].roomType != roomType
            ? index
            : GetRandomIndexOfRoomType(roomType, spawnedRooms);
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

    public void Unload()
    {
        _roomSpawnStarted = false;
        _roomSpawnCompleted = false;

        _roomsAmount = 0;
        _lootRoomsAmount = 0;

        _roomTemplates.Unload();
    }
}
