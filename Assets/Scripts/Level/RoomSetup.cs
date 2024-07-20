using System.Collections.Generic;
using UnityEngine;

public class RoomSetup : MonoBehaviour
{
    private RoomTemplates templates;
    private List<GameObject> enemiesInRoom;
    public List<GameObject> doors;
    private GameObject contents;
    private bool bossRoom;

    public void OnFirstEnter()
    {
        foreach (GameObject enemy in enemiesInRoom)
        {
            enemy.GetComponent<EnemyAI>().enabled = true;
        }

        if (bossRoom)
        {
            Invoke("OnEnterBossRoom", 4);
        }
    }

    private void OnEnterBossRoom()
    {
        Audio.PlayBossMusic();

        foreach (GameObject enemy in enemiesInRoom)
        {
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (!health.showBossBar) return;
            GameObject newBossBar = Instantiate(templates.bossBar.gameObject);
            newBossBar.transform.SetParent(templates.bossBarGroup, false);

            Destroy(health.canvas);
            health.healthBar = newBossBar.GetComponentInChildren<SlicedFilledImage>();
        }
    }

    public void onEnemyDeath(GameObject enemy)
    {
        enemiesInRoom.Remove(enemy);

        //On room completed
        if (enemiesInRoom.Count <= 0)
        {
            if (!bossRoom)
            {
                GetComponent<Room>().completed = true;
                foreach (GameObject door in doors)
                {
                    door.GetComponent<Door>().MakeAccessible();
                }
            }
            else GetComponentInChildren<Elevator>().animator.SetBool("isOpen", true);
        }
    }

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        enemiesInRoom = new List<GameObject>();
        doors = new List<GameObject>();

        templates.spawnedRooms.Add(gameObject);

        //Instantiate doors
        foreach (RoomSpawner spawner in GetComponentsInChildren<RoomSpawner>())
        {
            GameObject doorPrefab = templates.doors[spawner.openingDirection - 1];
            GameObject door = Instantiate(doorPrefab, doorPrefab.transform.position + transform.position, doorPrefab.transform.rotation);
            door.transform.parent = transform;
            doors.Add(door);
        }

        //Instantiate contents
        contents = Instantiate(templates.floorPresets[Random.Range(0, templates.floorPresets.Length)], transform.position, Quaternion.identity);
        foreach (Enemy enemy in contents.GetComponentsInChildren<Enemy>())
        {
            enemiesInRoom.Add(enemy.gameObject);
            enemy.roomInside = this;
        }
        contents.transform.parent = transform;
    }

    public void MakeBossRoom()
    {
        enemiesInRoom.Clear();
        Destroy(contents);

        int contentIndex;
        if (FloorManager.floor > templates.bossRooms.Length - 1) contentIndex = templates.bossRooms.Length - 1;
        else contentIndex = FloorManager.floor;
        contents = Instantiate(templates.bossRooms[contentIndex], transform.position, Quaternion.identity);
        foreach (Enemy enemy in contents.GetComponentsInChildren<Enemy>())
        {
            enemiesInRoom.Add(enemy.gameObject);
            enemy.roomInside = this;
        }
        contents.transform.parent = transform;

        GameObject elevator = Instantiate(templates.elevator, transform.position, Quaternion.identity);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Interactables>().elevator = elevator.GetComponent<Elevator>();
        elevator.transform.parent = contents.transform;

        bossRoom = true;
    }
}
