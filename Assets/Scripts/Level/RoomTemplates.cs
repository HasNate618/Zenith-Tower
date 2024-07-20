using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject[] bossRooms;

    public List<GameObject> spawnedRooms;

    public int maxRooms;
    [SerializeField] float waitTime;
    public GameObject elevator;

    private void Awake()
    {
        roomPresets[0] = presets1;
        roomPresets[1] = presets2;
        roomPresets[2] = presets3;
        roomPresets[3] = presets4;
        roomPresets[4] = presets5;

        int floor = FloorManager.floor;
        if (floor > 0)
        {
            if (floor < roomPresets.Length)
            {
                floorPresets = roomPresets[floor - 1].Concat(roomPresets[floor]).ToArray();
            }
            else
            {
                floorPresets = roomPresets[roomPresets.Length - 2].Concat(roomPresets[roomPresets.Length - 1]).ToArray();
            }
        }
        else floorPresets = roomPresets[0];

        maxRooms = floor * 2 + 12;

        StartCoroutine(SpawnBossRoom());
    }

    private IEnumerator SpawnBossRoom()
    {
        float elapsed = 0;
        while (elapsed < waitTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        spawnedRooms[spawnedRooms.Count - 1].GetComponent<RoomSetup>().MakeBossRoom();
    }

    ////// CONTENTS //////

    public GameObject[] floorPresets;
    private GameObject[][] roomPresets = new GameObject[5][];
    [SerializeField] GameObject[] presets1, presets2, presets3, presets4, presets5;
    public GameObject[] doors;

    ////// OTHER //////

    public RectTransform bossBar, bossBarGroup;
}
