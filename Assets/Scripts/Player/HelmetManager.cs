using UnityEngine;

public class HelmetManager : MonoBehaviour
{
    [SerializeField] MeshFilter helmetMesh;
    public Mesh[] helmets;

    void Start()
    {
        SetHelmet(99);
    }

    public void SetHelmet(int index)
    {
        if (index == 99) helmetMesh.mesh = helmets[PlayerPrefs.GetInt("Helmet", 1)];
        else helmetMesh.mesh = helmets[index];
    }
}
