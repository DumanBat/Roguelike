using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class Room : MonoBehaviour
{
    public RoomTemplates.RoomType roomType;
    [SerializeField]
    private List<GameObject> _doorTop, _doorBottom, _doorLeft, _doorRight;
    private List<GameObject>[] _roomDoors = new List<GameObject>[4];
    private Room[] _sideRooms; // Indexes: 0 - Top room, 1 - Bottom, 2 - Right, 3 - Left

    public List<RoomSpawnPoint> roomSpawnPoints; 
    private RoomTemplates _roomTemplates;

    private List<Enemy> _spawnedEnemies;

    public Action onRoomCleared;

    private void Awake()
    {
        roomSpawnPoints.AddRange(GetComponentsInChildren<RoomSpawnPoint>());
        _roomTemplates = GameManager.Instance.levelManager.GetRoomTemplates();

        _roomDoors[0] = _doorBottom.Count > 0 ? _doorBottom : null;
        _roomDoors[1] = _doorTop.Count > 0 ? _doorTop : null;
        _roomDoors[2] = _doorLeft.Count > 0 ? _doorLeft : null;
        _roomDoors[3] = _doorRight.Count > 0 ? _doorRight : null;
    }

    public void Init()
    {
        if (roomSpawnPoints.Count == 0) return;

        _roomTemplates.spawnedRooms.Add(this);
        onRoomCleared += OpenDoors;
        gameObject.transform.SetParent(NavMeshController.Instance.transform);
        GameManager.Instance.levelManager.SetLastSpawnedRoomTime(Time.time);
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

    public void SpawnEnemies(List<EnemyType> enemiesToSpawn)
    {
        if (enemiesToSpawn.Count == 0) return;

        var levelManager = GameManager.Instance.levelManager;
        _spawnedEnemies = new List<Enemy>();

        foreach (var enemyType in enemiesToSpawn)
        {
            var randomPosX = transform.position.x + UnityEngine.Random.Range(0, 6);
            var randomPosY = transform.position.y + UnityEngine.Random.Range(0, 6);
            var spawnPosition = new Vector2(randomPosX, randomPosY);

            var enemy = levelManager.SpawnEnemy(enemyType, spawnPosition);
            enemy.OriginRoom = this;
            _spawnedEnemies.Add(enemy);
        }
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
}
