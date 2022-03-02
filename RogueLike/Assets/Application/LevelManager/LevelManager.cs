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
    private RoomTemplates _roomTemplates;
    private LootManager _lootManager;

    private float _lastSpawnedRoomTime = -9999f;
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

        /*_lootManager.SpawnWeapon(_lootManager.weaponToSpawn, new Vector3(3f, 0f, 0f));
        _lootManager.SpawnWeapon(_lootManager.weaponToSpawn, new Vector3(-1, 1f, 0f));*/

        _roomSpawnStarted = true;
    }

    public void RandomizeLevelConfig()
    {
        _roomsAmount = UnityEngine.Random.Range(10, 16);
        _lootRoomsAmount = UnityEngine.Random.Range(1, 3);
    }

    private void ConfigureRooms()
    {
        // only enemies config
        _startingRoom.OpenDoors();

        var spawnedRooms = _roomTemplates.spawnedRooms;
        var lootRoomIndexes = new List<int>();

        for (int i = 0; i < _lootRoomsAmount; i++)
            lootRoomIndexes.Add(UnityEngine.Random.Range(0, spawnedRooms.Count));

        for (int i = 0; i < spawnedRooms.Count; i++)
        {
            if (lootRoomIndexes.Contains(i))
            {
                var weapon = _lootManager.SpawnWeapon(_lootManager.weaponToSpawn, spawnedRooms[i].transform.position);
                //weapon.onWeaponPickUp += spawnedRooms[i].OpenDoors();
            }
            else
            {
                var enemiesToSpawn = ConfigureEnemies();

                spawnedRooms[i].SpawnEnemies(enemiesToSpawn);
            }
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

    public void Unload()
    {
        _roomSpawnStarted = false;
        _roomSpawnCompleted = false;

        _roomsAmount = 0;
        _lootRoomsAmount = 0;

        _roomTemplates.Unload();
    }
}
