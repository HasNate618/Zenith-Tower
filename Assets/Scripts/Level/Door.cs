using UnityEngine;

public class Door : MonoBehaviour
{
    public bool accessible;
    public Transform buttonPlace;
    public bool opened;
    public Room room;

    [SerializeField] Transform fillWall;
    [SerializeField] Transform terminal;
    public Door adjacentDoor;
    public int openingDirection;
    /*  
    1 --> need bottom door
    2 --> need top door
    3 --> need left door
    4 --> need right door
    */

    private void Awake()
    {
        buttonPlace = transform.Find("ButtonPoint");

        Invoke("GetAdjacentDoor", 2);
    }

    private void GetAdjacentDoor()
    {
        room = GetComponentInParent<Room>();


        if (GetComponentInParent<Room>().neighborRooms[openingDirection - 1] == null)
        {
            CloseOff();
            return;
        }

        Door[] doors = GetComponentInParent<Room>().neighborRooms[openingDirection - 1].GetComponentsInChildren<Door>();
        int[] oppositeDirectionOf = new int[] { 2, 1, 4, 3 };
        foreach (Door door in doors)
        {
            if (door.openingDirection == oppositeDirectionOf[openingDirection - 1])
            {
                adjacentDoor = door;
            }
        }
        if (adjacentDoor == null)
        {
            CloseOff();
        }
    }

    private void CloseOff()
    {
        Transform newWall = Instantiate(fillWall, transform.position, transform.rotation);
        newWall.parent = GameObject.FindGameObjectWithTag("Level").transform;
        Instantiate(terminal, newWall);

        GetComponentInParent<RoomSetup>().doors.Remove(gameObject);
        Destroy(gameObject);
    }

    public void MakeAccessible()
    {
        if (!room.completed) return;

        if (adjacentDoor.room.completed) adjacentDoor.OpenDoor();
        else accessible = true;
    }

    public void OpenDoor()
    {
        if (!opened)
        {
            //open door
            GetComponent<Animation>().Play("DoorOpen");
            opened = true;

            adjacentDoor.OpenDoor();
        }
    }

    public void CloseDoor()
    {
        if (opened)
        {
            GetComponent<Animation>().Play("DoorClose");
            opened = false;

            adjacentDoor.CloseDoor();
            Audio.PlaySound("DoorSlide", null);
        }
    }
}
