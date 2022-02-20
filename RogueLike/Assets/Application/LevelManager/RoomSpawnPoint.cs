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
        _roomTemplates = GameManager.Instance.levelManager.GetRoomTemplates();
    }

    public void SpawnRoom()
    {
        if (_spawned) return;
        if (openingDirection == 0)
        {
            _spawned = true;
            return;
        }

        Room[] currentRoomList = null;

        switch (openingDirection)
        {
            case 1: 
                currentRoomList = _roomTemplates.bottomRooms;
                break;
            case 2:
                currentRoomList = _roomTemplates.topRooms;
                break;
            case 3:
                currentRoomList = _roomTemplates.leftRooms;
                break;
            case 4:
                currentRoomList = _roomTemplates.rightRooms;
                break;
        }

        var rand = Random.Range(0, currentRoomList.Length);
        var room = Instantiate(currentRoomList[rand], transform.position, Quaternion.identity);
        room.Init();

        _spawned = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("SpawnPoint")) return;

        var roomSpawnPoint = collision.GetComponent<RoomSpawnPoint>();

        if (roomSpawnPoint != null)
        {
            if (roomSpawnPoint._spawned == false && _spawned == false)
            {
                Instantiate(_roomTemplates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            _spawned = true;
        }
    }
}
