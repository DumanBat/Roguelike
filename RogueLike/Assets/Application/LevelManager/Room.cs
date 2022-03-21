using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class Room : MonoBehaviour
{
    private string _playerTag = "Player";
    [SerializeField]
    public RoomTemplates.RoomType roomType;
    public Transform spawnPointsRoot;
    [SerializeField]
    private List<GameObject> _doorTop, _doorBottom, _doorLeft, _doorRight;
    private List<GameObject>[] _roomDoors = new List<GameObject>[4];
    private Room[] _sideRooms; // Indexes: 0 - Top room, 1 - Bottom, 2 - Right, 3 - Left
    [SerializeField]
    private GameObject _passToNextLevel;

    [SerializeField]
    private List<RoomSpawnPoint> roomSpawnPoints;

    public Transform spawnedEnemiesRoot;
    private List<Enemy> _spawnedEnemies;

    private BoxCollider2D _roomCollider;
    public Action onRoomCleared;

    private void Awake()
    {
        roomSpawnPoints.AddRange(spawnPointsRoot.GetComponentsInChildren<RoomSpawnPoint>());
        _roomCollider = GetComponent<BoxCollider2D>();
    }

    public void Init()
    {
        if (roomSpawnPoints.Count == 0) return;

        _roomDoors[0] = _doorBottom.Count > 0 ? _doorBottom : null;
        _roomDoors[1] = _doorTop.Count > 0 ? _doorTop : null;
        _roomDoors[2] = _doorLeft.Count > 0 ? _doorLeft : null;
        _roomDoors[3] = _doorRight.Count > 0 ? _doorRight : null;

        onRoomCleared += OpenDoors;
        gameObject.transform.SetParent(NavMeshController.Instance.transform);
        GameManager.Instance.levelManager.GetLevelConfigurator().SetLastSpawnedRoomTime(Time.time);
        Invoke("SpawnSideRooms", 0.3f);
    }

    public void SpawnSideRooms()
    {
        _sideRooms = new Room[4];

        foreach (var spawnPoint in roomSpawnPoints)
        {
            var room = spawnPoint.SpawnRoom();

            if (room != null)
                _sideRooms[spawnPoint.openingDirection - 1] = room;
        }
    }

    public List<Enemy> SpawnEnemies(List<EnemyType> enemiesToSpawn)
    {
        if (enemiesToSpawn.Count == 0) return null;

        var levelConfigurator = GameManager.Instance.levelManager.GetLevelConfigurator();
        _spawnedEnemies = new List<Enemy>();

        foreach (var enemyType in enemiesToSpawn)
        {
            var randomPosX = transform.position.x + UnityEngine.Random.Range(0, 6);
            var randomPosY = transform.position.y + UnityEngine.Random.Range(0, 6);
            var spawnPosition = new Vector2(randomPosX, randomPosY);

            var enemy = levelConfigurator.SpawnEnemy(enemyType, spawnPosition);
            enemy.transform.SetParent(spawnedEnemiesRoot);
            enemy.OriginRoom = this;
            _spawnedEnemies.Add(enemy);
        }

        return _spawnedEnemies;
    }

    public bool RemoveFromSpawnedEnemiesList(Enemy enemy)
    {
        if (_spawnedEnemies.Count - 1 == 0) 
            onRoomCleared.Invoke();
        return _spawnedEnemies.Remove(enemy);
    }

    public void OpenDoors()
    {
        foreach (var door in _roomDoors)
        {
            if (door == null)
                continue;

            door[0].SetActive(false);
            door[1].SetActive(true);
        }

        for (int i = 0; i < _sideRooms.Length; i++)
        {
            if (_sideRooms[i] == null)
                continue;
            if (_sideRooms[i]._roomDoors[i] == null)
                continue;

            _sideRooms[i]._roomDoors[i][0].SetActive(false);
            _sideRooms[i]._roomDoors[i][1].SetActive(true);
        }
    }

    public void CloseDoors()
    {
        foreach (var door in _roomDoors)
        {
            if (door == null)
                continue;

            door[0].SetActive(true);
            door[1].SetActive(false);
        }
    }

    public void OpenNextLevelPass()
    {
        if (roomType != RoomTemplates.RoomType.BossRoom)
            return;

        _passToNextLevel.SetActive(true);

        var pushDirection = PlayerController.Instance.GetPosition() - new Vector2(transform.position.x, transform.position.y);
        StartCoroutine(PlayerController.Instance.GetPush(pushDirection.normalized * 10f, 1.5f));
    }

    public void Unload()
    {
        _sideRooms = null;

        if (_spawnedEnemies != null)
        {
            foreach (var enemy in _spawnedEnemies)
                Destroy(enemy.gameObject);
            _spawnedEnemies.Clear();
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_playerTag))
        {
            CloseDoors();
            Destroy(_roomCollider);
        }
    }
}
