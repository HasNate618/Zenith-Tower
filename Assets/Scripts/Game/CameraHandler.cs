using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] Transform cameraTarget;
    [SerializeField] float followSpeed;

    private void Update()
    {
        if (cameraTarget != null)
        {
            transform.position = Vector3.Slerp(transform.position, cameraTarget.position, followSpeed * Time.deltaTime);
        }
    }
}
