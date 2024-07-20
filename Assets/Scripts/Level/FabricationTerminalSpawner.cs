using UnityEngine;

public class FabricationTerminalSpawner : MonoBehaviour
{
    [SerializeField] Vector3[] spawnPoints;
    [SerializeField] Transform fabricationTerminalPrefab;

    // Start is called before the first frame update
    void Start()
    {
        int randomNumber = Random.Range(0, 4);
        Instantiate(fabricationTerminalPrefab, spawnPoints[randomNumber], Quaternion.Euler(0, randomNumber * 90, 0)).transform.parent = transform;
        Destroy(this);
    }
}
