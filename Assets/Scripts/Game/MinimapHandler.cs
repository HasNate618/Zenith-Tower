using UnityEngine;

public class MinimapHandler : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 currentPosition;
    [SerializeField] Vector3 positionOffset;
    [SerializeField] float transitionSpeed;

    void Update()
    {
        if (CharacterRoomPosition() != currentPosition) currentPosition = CharacterRoomPosition();

        if (Vector3.Distance(transform.position - positionOffset, currentPosition) > 1)
        {
            transform.position = Vector3.Lerp(transform.position, currentPosition + positionOffset, transitionSpeed);
        } else transform.position = currentPosition + positionOffset;
    }

    private Vector3 CharacterRoomPosition()
    {
        float roundedX = Mathf.Round(player.position.x / 35) * 35;
        float roundedZ = Mathf.Round(player.position.z / 20) * 20;
        return new Vector3(roundedX, 0, roundedZ);
    }
}
