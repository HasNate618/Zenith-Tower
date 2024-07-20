using UnityEngine;

public class SpawnRandomRobot : MonoBehaviour
{
    [SerializeField] Transform[] robots;
    [SerializeField] int minSpawnTime, maxSpawnTime;

    // Start is called before the first frame update
    void Awake()
    {
        SetNextSpawnTime();
    }

    private void SpawnRobot()
    {
        Instantiate(robots[Random.Range(0, robots.Length)], transform);
        SetNextSpawnTime();
    }

    private void SetNextSpawnTime()
    {
        Invoke("SpawnRobot", Random.Range(minSpawnTime, maxSpawnTime));
    }
}
