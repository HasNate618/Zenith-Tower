using UnityEngine;

public class CharacterAnimEvents : MonoBehaviour
{
    [SerializeField] CharacterMovement movement;

    public void EndRoll()
    {
        movement.EndRoll();
    }


    [SerializeField] Interactables interactables;

    public void StopButtonPress()
    {
        movement.characterAnimator.SetBool("ButtonPress", false);
    }

    public void OpenDoor()
    {
        if (interactables.closestDoor != null)
        {
            interactables.closestDoor.OpenDoor();
            PlaySound("DoorSlide");
        }
        PlaySound("ButtonNoise");
    }


    [SerializeField] FabricationManager fabrication;

    public void OpenFabricationMenu()
    {
        fabrication.OpenFabricationMenu();
    }

    public void EndInteraction()
    {
        fabrication.EndInteraction();
    }

    public void PlaySound(string name)
    {
        Audio.PlaySound(name, null);
    }
}
