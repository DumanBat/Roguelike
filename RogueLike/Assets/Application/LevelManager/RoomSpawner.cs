using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;

    private RoomTemplates _roomTemplates;
    private bool _spawned = false;

    public float waitTime = 5f;

    private void Start()
    {
        //Destroy(gameObject, waitTime);
        _roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        Invoke("SpawnRooms", 0.3f);
    }

    private void SpawnRooms()
    {
        if (_spawned) return;

        if (openingDirection == 1)
        {
            var rand = Random.Range(0, _roomTemplates.bottomRooms.Length);
            Instantiate(_roomTemplates.bottomRooms[rand], transform.position, Quaternion.identity);
        }
        else if (openingDirection == 2)
        {
            var rand = Random.Range(0, _roomTemplates.topRooms.Length);
            Instantiate(_roomTemplates.topRooms[rand], transform.position, Quaternion.identity);
        }
        else if (openingDirection == 3)
        {
            var rand = Random.Range(0, _roomTemplates.leftRooms.Length);
            Instantiate(_roomTemplates.leftRooms[rand], transform.position, Quaternion.identity);
        }
        else if (openingDirection == 4)
        {
            var rand = Random.Range(0, _roomTemplates.rightRooms.Length);
            Instantiate(_roomTemplates.rightRooms[rand], transform.position, Quaternion.identity);
        }

        _spawned = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint"))
        {
            if (collision.GetComponent<RoomSpawner>() != null)
            {
                if (collision.GetComponent<RoomSpawner>()._spawned == false && _spawned == false)
                {
                    Debug.LogWarning("Spawned - " + gameObject.transform.parent.name);
                    Instantiate(_roomTemplates.closedRoom, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                _spawned = true;
            }
        }
    }
}
