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
        _roomTemplates.spawnedRooms.Add(this);
        Invoke("SpawnSideRooms", 0.3f);
    }

    public void SpawnSideRooms()
    {
        foreach (var spawnPoint in roomSpawnPoints)
        {
            spawnPoint.SpawnRoom();
        }
    }
}
