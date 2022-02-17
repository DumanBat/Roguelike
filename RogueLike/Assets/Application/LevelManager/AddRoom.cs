using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private RoomTemplates _roomTemplates;

    private void Start()
    {
        _roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        _roomTemplates.rooms.Add(this.gameObject);
    }
}
