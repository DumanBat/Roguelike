using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public Room[] startingRooms;

    public Room[] bottomRooms;
    public Room[] topRooms;
    public Room[] leftRooms;
    public Room[] rightRooms;

    public Room closedRoom;

    public List<Room> spawnedRooms;
}
