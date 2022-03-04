using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    [SerializeField]
    private Room _startingRoom, _closedRoom;
    public Room GetStartingRoom() => _startingRoom;
    public Room GetClosedRoom() => _closedRoom;
    [SerializeField]
    private Room[] _bottomRooms, _topRooms, _leftRooms, _rightRooms, _bossRoom;
    public Room[] GetBossRoom() => _bossRoom;

    public List<Room> spawnedRooms;

    public enum RoomType
    {
        EnemyRoom,
        LootRoom,
        BossRoom
    }

    public Room[] GetRoomsBySide(int side)
    {
        return side switch
        {
            0 => _bottomRooms,
            1 => _topRooms,
            2 => _leftRooms,
            3 => _rightRooms,
            _ => null
        };
    }

    public void Init(Room startingRoom, Room closedRoom, Room[] bossRoom,
        Room[] bottomRooms, Room[] topRooms, Room[] leftRooms, Room[] rightRooms)
    {
        _startingRoom = startingRoom;
        _closedRoom = closedRoom;
        _bossRoom = bossRoom;
        _bottomRooms = bottomRooms;
        _topRooms = topRooms;
        _leftRooms = leftRooms;
        _rightRooms = rightRooms;
    }

    public void Unload()
    {
        _startingRoom = null;
        _closedRoom = null;
        _bossRoom = null;
        _bottomRooms = null;
        _topRooms = null;
        _leftRooms = null;
        _rightRooms = null;

        spawnedRooms.Clear();
    }
}
