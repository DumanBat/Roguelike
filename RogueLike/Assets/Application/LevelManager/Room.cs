using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class Room : MonoBehaviour
{
    public List<RoomSpawnPoint> roomSpawnPoints; 
    private RoomTemplates _roomTemplates;

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

    public void SpawnEnemies()
    {
        GameManager.Instance.levelManager.SpawnEnemy(EnemyType.GreenJelly, new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f));
        GameManager.Instance.levelManager.SpawnEnemy(EnemyType.PinkJelly, new Vector2(transform.position.x + 1f, transform.position.y + 1f));
        GameManager.Instance.levelManager.SpawnEnemy(EnemyType.PinkJelly, new Vector2(transform.position.x + 2f, transform.position.y + 2f));
    }
}
