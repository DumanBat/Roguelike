using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;

    private RoomTemplates _roomTemplates;
    private bool _spawned = false;

    private void Start()
    {
        //Destroy(gameObject, waitTime);
        _roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        Invoke("SpawnRooms", 0.3f);
    }

    private void SpawnRooms()
    {
        if (_spawned) return;

        GameObject[] currentRoomList = null;

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
        
        if (currentRoomList != null)
        {
            var rand = Random.Range(0, currentRoomList.Length);
            Instantiate(currentRoomList[rand], transform.position, Quaternion.identity);
        }

        _spawned = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("SpawnPoint")) return;

        var roomSpawner = collision.GetComponent<RoomSpawner>();

        if (roomSpawner != null)
        {
            if (roomSpawner._spawned == false && _spawned == false)
            {
                Instantiate(_roomTemplates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            _spawned = true;
        }
    }
}
