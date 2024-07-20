using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<int> openings;
    public Room[] neighborRooms = new Room[4];

    public bool completed;

}
