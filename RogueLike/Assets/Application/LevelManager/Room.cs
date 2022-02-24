using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class Room : MonoBehaviour
{
    public List<RoomSpawnPoint> roomSpawnPoints; 
    private RoomTemplates _roomTemplates;

    private List<Enemy> _spawnedEnemies;

    private bool _isRoomCleared;
    public bool IsCleared() => _isRoomCleared;

    private void Awake()
    {
        roomSpawnPoints.AddRange(GetComponentsInChildren<RoomSpawnPoint>());
        _roomTemplates = GameManager.Instance.levelManager.GetRoomTemplates();
    }

    public void Init()
    {
        if (roomSpawnPoints.Count == 0) return;

        _roomTemplates.spawnedRooms.Add(this);
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
        var levelManager = GameManager.Instance.levelManager;
        _spawnedEnemies = new List<Enemy>();

        foreach (var enemyType in enemiesToSpawn)
        {
            var randomPosX = transform.position.x + Random.Range(0, 6);
            var randomPosY = transform.position.y + Random.Range(0, 6);
            var spawnPosition = new Vector2(randomPosX, randomPosY);

            var enemy = levelManager.SpawnEnemy(enemyType, spawnPosition);
            enemy.OriginRoom = this;
            _spawnedEnemies.Add(enemy);
        }
    }

    public bool RemoveEnemyFromSpawnedEnemyList(Enemy enemy)
    {
        return _spawnedEnemies.Remove(enemy);
    }
}
