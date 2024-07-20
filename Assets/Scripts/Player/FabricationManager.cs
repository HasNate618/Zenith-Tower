using System.Collections;
using UnityEngine;

public class FabricationManager : MonoBehaviour
{
    [SerializeField] float checkRadius, lerpSpeed;
    [SerializeField] LayerMask terminalLayer;

    [SerializeField] GameObject interactButton;
    public bool startingInteraction, isInteracting, endingInteraction;

    [SerializeField] CharacterMovement movement;

    [SerializeField] GameObject fabricationMenu, inventoryPanel, gameInterface;

    public Transform fabricationSlot;
    [SerializeField] Transform batteryItem;

    private void Update()
    {
        if (Time.frameCount % 5 != 0) return;

        //Shows interaction button if in range of terminal
        interactButton.SetActive(Physics.CheckSphere(transform.position, checkRadius, terminalLayer, QueryTriggerInteraction.Collide));
    }

    public void OnInteract()
    {
        if (!movement.isRolling)
        {
            startingInteraction = true;
            StartCoroutine(StartInteraction());
        }
    }

    private IEnumerator StartInteraction()
    {
        gameInterface.SetActive(false);
        GetComponent<PlayerHealth>().invincible = true;
        movement.enabled = false;
        movement.armature.localRotation = Quaternion.Euler(0, 0, 0);
        CharacterShooting shooting = GetComponent<CharacterShooting>();
        shooting.CancelAiming();
        shooting.enabled = false;

        Collider[] collisions = Physics.OverlapSphere(transform.position, checkRadius, terminalLayer, QueryTriggerInteraction.Collide);
        Transform closestTerminal = null;
        if (collisions.Length > 0)
        {
            closestTerminal = collisions[0].transform;
        }

        while (startingInteraction)
        {
            transform.position = Vector3.Lerp(transform.position, closestTerminal.position, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, closestTerminal.rotation, lerpSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, closestTerminal.position) < 0.05f)
            {
                transform.rotation = closestTerminal.rotation;
                movement.characterAnimator.SetBool("isFabricating", true);
                startingInteraction = false;
                isInteracting = true;
            }
            yield return null;
        }
    }

    public void OpenFabricationMenu()
    {
        fabricationMenu.SetActive(true);
        inventoryPanel.SetActive(true);
        Time.timeScale = 0;
        Audio.musicSource.Pause();
    }

    public void CloseFabricationMenu()
    {
        if (Input.touchCount > 1) return;
        fabricationMenu.SetActive(false);
        inventoryPanel.SetActive(false);
        Time.timeScale = 1;
        Audio.musicSource.UnPause();
        Audio.PlaySound("InterfaceInteraction1", null);
    }

    public void EndInteraction()
    {
        movement.characterAnimator.SetBool("isFabricating", false);
        movement.enabled = true;
        GetComponent<CharacterShooting>().enabled = true;
        GetComponent<PlayerHealth>().invincible = false;
        gameInterface.SetActive(true);
    }

    public void FabricateBattery()
    {
        if (fabricationSlot.childCount > 0)
        {
            Audio.PlaySound("InterfaceError", null);
            return;
        }

        if (CreditsManager.attemptCredits >= 800 && fabricationSlot.childCount == 0)
        {
            CreditsManager.attemptCredits -= 800;
            CreditsManager.instance.UpdateCreditUI();

            Instantiate(batteryItem, fabricationSlot);
            Audio.PlaySound("ApplyEnhancement", null);
        }
        else
        {
            Audio.PlaySound("InterfaceError", null);
        }
    }
}
