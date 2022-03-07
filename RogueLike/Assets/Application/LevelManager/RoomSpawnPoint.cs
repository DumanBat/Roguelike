using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class RoomSpawnPoint : MonoBehaviour
{
    public int openingDirection;

    private RoomTemplates _roomTemplates;
    private bool _spawned = false;

    private void Awake()
    {
        _roomTemplates = GameManager.Instance.levelManager.GetLevelConfigurator().GetRoomTemplates();
    }

    public Room SpawnRoom()
    {
        if (_spawned) return null;
        if (openingDirection == 0)
        {
            _spawned = true;
            return null;
        }

        Room room;
        var roomsCount = _roomTemplates.activeRooms.Count;
        var roomsAmountToSpawn = GameManager.Instance.levelManager.GetLevelConfigurator().GetRoomsAmount();
        if (roomsCount < roomsAmountToSpawn)
        {
            room = roomsAmountToSpawn - 1 == roomsCount
               ? SpawnBossRoom()
               : SpawnRandomRoom();
        }
        else
        {
            room = Instantiate(_roomTemplates.GetClosedRoom(), transform.position, Quaternion.identity);
        }

        _roomTemplates.allRooms.Add(room);
        _spawned = true;
        return room;
    }

    private Room SpawnRandomRoom()
    {
        var currentRoomList = _roomTemplates.GetRoomsBySide(openingDirection - 1);

        var rand = Random.Range(0, currentRoomList.Length);
        var room = Instantiate(currentRoomList[rand], transform.position, Quaternion.identity);
        room.Init();
        _roomTemplates.activeRooms.Add(room);

        return room;
    }

    private Room SpawnBossRoom()
    {
        var room = Instantiate(_roomTemplates.GetBossRoom()[openingDirection - 1], transform.position, Quaternion.identity);
        room.roomType = RoomTemplates.RoomType.BossRoom;
        room.Init();
        _roomTemplates.activeRooms.Add(room);

        return room;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("SpawnPoint")) return;

        var roomSpawnPoint = collision.GetComponent<RoomSpawnPoint>();

        if (roomSpawnPoint != null)
        {
            if (roomSpawnPoint._spawned == false && _spawned == false)
            {
                var room = Instantiate(_roomTemplates.GetClosedRoom(), transform.position, Quaternion.identity);
                _roomTemplates.allRooms.Add(room);
                Destroy(gameObject);
            }
            _spawned = true;
        }
    }
}
