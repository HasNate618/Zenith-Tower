using System.Collections;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public GameObject openDoorButton;
    public Door closestDoor;
    public bool inRangeOfDoor, openingDoor, nearElevator;
    private int doorOpenSequence;
    private bool queueButtonShow;
    [SerializeField] float lerpSpeed;

    [SerializeField] CharacterMovement movement;
    [SerializeField] GameObject player;

    public Elevator elevator;

    public void OnTriggerEnter(Collider other)
    {
        Elevator elevator = other.GetComponentInParent<Elevator>();
        if (elevator != null)
        {
            if (!elevator.accessible) return;
            nearElevator = true;
            openDoorButton.SetActive(true);
            return;
        }

        Door door = other.transform.GetComponentInParent<Door>();
        if (door == null || !door.accessible || door.opened) return;

        if (openingDoor) queueButtonShow = true;
        else openDoorButton.SetActive(true);

        inRangeOfDoor = true;
        closestDoor = door;
    }

    private void OnTriggerExit(Collider other)
    {
        if (nearElevator)
        {
            openDoorButton.SetActive(false);
            return;
        }

        if (closestDoor == null) return;

        openDoorButton.SetActive(false);
        inRangeOfDoor = false;
        if (!openingDoor) closestDoor = null;
    }

    public void OpenDoor()
    {
        if (closestDoor != null && !movement.isRolling)
        {
            //open door
            openDoorButton.SetActive(false);
            openingDoor = true;
            StartCoroutine(OpenDoorSequence());
            GetComponentInParent<PlayerHealth>().invincible = true;
            return;
        }

        if (nearElevator)
        {
            elevator.Interact();
            openDoorButton.SetActive(false);
        }
    }

    private IEnumerator OpenDoorSequence()
    {
        movement.enabled = false;
        movement.armature.localRotation = Quaternion.Euler(0, 0, 0);
        CharacterShooting shooting = player.GetComponent<CharacterShooting>();
        shooting.CancelAiming();
        shooting.enabled = false;
        closestDoor.adjacentDoor.GetComponentInParent<RoomSetup>().OnFirstEnter();
        movement.joysticks[0].SetActive(false);
        movement.joysticks[2].SetActive(false);

        while (openingDoor)
        {
            switch (doorOpenSequence)
            {
                case 0: //Lerp to button position
                    player.transform.position = Vector3.Lerp(player.transform.position, closestDoor.buttonPlace.position, lerpSpeed * Time.deltaTime);
                    player.transform.rotation = Quaternion.Lerp(player.transform.rotation, closestDoor.buttonPlace.rotation, lerpSpeed * Time.deltaTime);
                    if (Vector3.Distance(player.transform.position, closestDoor.buttonPlace.position) < 0.05f)
                    {
                        player.transform.rotation = closestDoor.buttonPlace.rotation;
                        doorOpenSequence++;
                        movement.characterAnimator.SetBool("ButtonPress", true);
                        yield return new WaitForSeconds(2);
                    }
                    break;

                case 1: //Lerp in front of door
                    if (Vector3.Distance(player.transform.position, closestDoor.transform.position - closestDoor.transform.forward) > 0.05f)
                    {
                        player.transform.position = Vector3.Lerp(player.transform.position, closestDoor.transform.position - closestDoor.transform.forward, lerpSpeed * Time.deltaTime);
                    }
                    else
                    {
                        movement.characterAnimator.SetBool("isWalking", true);
                        doorOpenSequence++;
                        closestDoor.Invoke("CloseDoor", 1.5f);
                    }
                    break;

                case 2: //Walk inside next room and close door
                    if (Vector3.Distance(player.transform.position, closestDoor.transform.position + closestDoor.transform.forward) > 0.1f)
                    {
                        player.transform.position += player.transform.forward * Time.deltaTime * 1.5f;
                    }
                    else
                    {
                        movement.characterAnimator.SetBool("isWalking", false);

                        closestDoor = null;
                        inRangeOfDoor = false;
                        doorOpenSequence++;

                        yield return new WaitForSeconds(1);

                        openingDoor = false;
                        doorOpenSequence = 0;
                        movement.enabled = true;
                        player.GetComponent<CharacterShooting>().enabled = true;
                        player.GetComponent<PlayerHealth>().invincible = false;


                        if (queueButtonShow)
                        {
                            queueButtonShow = false;
                            openDoorButton.SetActive(true);
                        }

                        yield break;
                    }
                    break;
            }
            yield return null;
        }
    }
}
