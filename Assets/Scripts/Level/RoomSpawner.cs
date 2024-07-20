using UnityEngine;
using System.Linq;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    /*  
        1 --> need bottom door
        2 --> need top door
        3 --> need left door
        4 --> need right door
    */

    private RoomTemplates templates;
    private int rand;
    private bool spawned;

    public float waitTime = 5f;

    GameObject[] roomsThis;
    GameObject[] roomsOther;

    private void Start()
    {
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", Random.Range(0.1f, 0.4f));
        //GetComponentInParent<Room>().openings.Add(openingDirection);
    }

    private void Spawn()
    {
        if (templates.spawnedRooms.Count >= templates.maxRooms)
        {
            return;
        }

        if (!spawned)
        {
            GameObject newRoom;
            switch (openingDirection)
            {
                case 1:
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    newRoom = Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                    newRoom.transform.parent = GameObject.FindGameObjectWithTag("Level").transform;
                    break;                
                case 2:
                    rand = Random.Range(0, templates.topRooms.Length);
                    newRoom = Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                    newRoom.transform.parent = GameObject.FindGameObjectWithTag("Level").transform;
                    break;                
                case 3:
                    rand = Random.Range(0, templates.leftRooms.Length);
                    newRoom = Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                    newRoom.transform.parent = GameObject.FindGameObjectWithTag("Level").transform;
                    break;                
                case 4:
                    rand = Random.Range(0, templates.rightRooms.Length);
                    newRoom = Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                    newRoom.transform.parent = GameObject.FindGameObjectWithTag("Level").transform;
                    break;
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            if (openingDirection > 0)
            {
                GetComponentInParent<Room>().neighborRooms[openingDirection - 1] = other.GetComponent<Room>();
            }
        }
        if (other.CompareTag("SpawnPoint"))
        {
            if (!other.GetComponent<RoomSpawner>().spawned && !spawned)
            {
                if (templates == null) templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
                switch (openingDirection)
                {
                    case 0:
                        spawned = true;
                        return;
                    case 1:
                        roomsThis = templates.bottomRooms;
                        break;
                    case 2:
                        roomsThis = templates.topRooms;
                        break;
                    case 3:
                        roomsThis = templates.leftRooms;
                        break;
                    case 4:
                        roomsThis = templates.rightRooms;
                        break;
                }
                switch (other.GetComponent<RoomSpawner>().openingDirection)
                {
                    case 0:
                        Destroy(gameObject);
                        return;
                    case 1:
                        roomsOther = templates.bottomRooms;
                        break;
                    case 2:
                        roomsOther = templates.topRooms;
                        break;
                    case 3:
                        roomsOther = templates.leftRooms;
                        break;
                    case 4:
                        roomsOther = templates.rightRooms;
                        break;
                }

                if (roomsThis.Intersect(roomsOther).Any())
                {
                    var matchingRoom = roomsThis.Intersect(roomsOther);
                    GameObject[] fitRoom = matchingRoom.ToArray();
                    GameObject newRoom = Instantiate(fitRoom[0], transform.position, fitRoom[0].transform.rotation);
                    newRoom.transform.parent = GameObject.FindGameObjectWithTag("Level").transform;
                }

                //Debug.Log("Room collided!");
            }

            spawned = true;
        } 
    }
}
