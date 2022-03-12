using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfigurator : MonoBehaviour
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
    public Action onRoomSpawnCompleted;

    private Room _startingRoom;

    // CONFIG
    [SerializeField]
    private int _roomsAmount;
    public int GetRoomsAmount() => _roomsAmount;
    private int _lootRoomsAmount;
    private List<EnemyType> _bossPool;
    private List<EnemyType> _enemyPool;
    // TODO: ������ ������������� ������ �� Start
    // �������� ����� ��� ������ � ���� ������. ������� ��� � ������� � ������ � �� ������� ����� - FIXED
    // ����� ������� �� � ���� ���� (������ RoomType.BossRoom �� �� ������� � ������)

    private void Awake()
    {
        _roomTemplates = GetComponent<RoomTemplates>();
        _lootManager = GetComponent<LootManager>();
        /// TEMP
        _bossPool = new List<EnemyType>() { EnemyType.BossShark };
        _enemyPool = new List<EnemyType>() { EnemyType.GreenJelly, EnemyType.PinkJelly };
        /// TEMP
    }

    private void Update()
    {
        if (_roomSpawnCompleted) return;
        if (!_roomSpawnStarted) return;

        if (Time.time > _lastSpawnedRoomTime)
        {
            _roomSpawnCompleted = true;
            onRoomSpawnCompleted.Invoke();
            NavMeshController.Instance.Init();

            ConfigureRooms();
        }
    }

    public void SetLevelConfig()
    {
        _roomsAmount = UnityEngine.Random.Range(10, 16);
        _lootRoomsAmount = UnityEngine.Random.Range(1, 3);
        _bossPool = enemyFactory.GetBossesTypes();
        _enemyPool = enemyFactory.GetEnemiesTypes();
    }

    public void SetLevelConfig(int roomsAmount, int lootRoomsAmount, List<EnemyType> bossPool, List<EnemyType> enemyPool)
    {
        _roomsAmount = roomsAmount;
        _lootRoomsAmount = lootRoomsAmount;
        _bossPool = bossPool;
        _enemyPool = enemyPool;
    }

    public void Init()
    {
        _startingRoom = Instantiate(_roomTemplates.GetStartingRoom(), Vector3.zero, Quaternion.identity);
        _startingRoom.Init();
        _roomTemplates.allRooms.Add(_startingRoom);

        _lootManager.Init();
        _lootManager.SpawnWeapon(_lootManager.weaponToSpawn, new Vector3(3f, 0f, 0f));

        _roomSpawnStarted = true;
    }

    private void ConfigureRooms()
    {
        var spawnedRooms = _roomTemplates.activeRooms;
        //spawnedRooms[spawnedRooms.Count - 1].roomType = RoomTemplates.RoomType.BossRoom;

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
                case RoomTemplates.RoomType.BossRoom:
                    var bossType = _bossPool[UnityEngine.Random.Range(0, _bossPool.Count)];
                    var boss = spawnedRooms[i].SpawnEnemies(new List<EnemyType>() { bossType })[0];
                    boss.onEnemyDie += spawnedRooms[i].OpenNextLevelPass;
                    break;
            }
        }

        _startingRoom.OpenDoors();
    }

    private int GetRandomIndexOfRoomType(RoomTemplates.RoomType roomType, List<Room> spawnedRooms)
    {
        var index = UnityEngine.Random.Range(0, spawnedRooms.Count);
        return
            (spawnedRooms[index].roomType != roomType
            && spawnedRooms[index].roomType == RoomTemplates.RoomType.EnemyRoom)
            ? index
            : GetRandomIndexOfRoomType(roomType, spawnedRooms);
    }

    private List<EnemyType> ConfigureEnemies()
    {
        var enemiesAmountToSpawn = UnityEngine.Random.Range(2, 6);
        List<EnemyType> enemiesToSpawn = new List<EnemyType>();
        for (int i = 0; i < enemiesAmountToSpawn; i++)
        {
            var enemyType = _enemyPool[UnityEngine.Random.Range(0, _enemyPool.Count)];
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
        SetLastSpawnedRoomTime(-9999f);
        _roomSpawnStarted = false;
        _roomSpawnCompleted = false;

        _roomsAmount = 0;
        _lootRoomsAmount = 0;

        foreach (var room in _roomTemplates.allRooms)
            room.Unload();

        _roomTemplates.Unload();
    }
}
