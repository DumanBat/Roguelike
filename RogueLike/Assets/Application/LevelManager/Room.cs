using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class Room : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _doorTop, _doorBottom, _doorLeft, _doorRight;
    private List<List<GameObject>> _roomDoors = new List<List<GameObject>>();

    public List<RoomSpawnPoint> roomSpawnPoints; 
    private RoomTemplates _roomTemplates;

    private List<Enemy> _spawnedEnemies;

    public Action onRoomCleared;

    private void Awake()
    {
        roomSpawnPoints.AddRange(GetComponentsInChildren<RoomSpawnPoint>());
        _roomTemplates = GameManager.Instance.levelManager.GetRoomTemplates();
        _roomDoors.AddRange(new List<List<GameObject>>() 
            { _doorTop, _doorBottom, _doorLeft, _doorRight });
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
        foreach (var spawnPoint in roomSpawnPoints)
        {
            spawnPoint.SpawnRoom();
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
            if (door == null) return;

            door[0].SetActive(false);
            door[1].SetActive(true);
        }
    }
}
