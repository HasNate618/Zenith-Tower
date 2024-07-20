using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool accessible, interacting;
    private int interactSequence;
    [SerializeField] float lerpSpeed;
    [SerializeField] Transform buttonPlace, platform;
    public Animator animator;

    private GameObject player;
    private CharacterMovement movement;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void MakeAccessible()
    {
        accessible = true;
        if (Physics.CheckSphere(transform.position, 2, LayerMask.NameToLayer("Player")))
        {
            player.GetComponent<Interactables>().OnTriggerEnter(GetComponentInChildren<Collider>());
        }
    }

    public void Interact()
    {
        interacting = true;
        StartCoroutine(InteractSequence());
    }

    private IEnumerator InteractSequence()
    {
        while (interacting)
        {
            switch (interactSequence)
            {
                case 0: //On Interact
                    GameObject.FindGameObjectWithTag("Canvas").transform.Find("GameUI").gameObject.SetActive(false);
                    movement = player.GetComponent<CharacterMovement>();
                    movement.enabled = false;
                    movement.armature.localRotation = Quaternion.Euler(0, 0, 0);
                    CharacterShooting shooting = player.GetComponent<CharacterShooting>();
                    shooting.CancelAiming();
                    shooting.enabled = false;
                    interactSequence++;
                    break;

                case 1: //Lerp to button position
                    player.transform.position = Vector3.Lerp(player.transform.position, buttonPlace.position, lerpSpeed * Time.deltaTime);
                    player.transform.rotation = Quaternion.Lerp(player.transform.rotation, buttonPlace.rotation, lerpSpeed * Time.deltaTime);
                    if (Vector3.Distance(player.transform.position, buttonPlace.position) < 0.05f)
                    {
                        //Press button
                        player.transform.rotation = buttonPlace.rotation;
                        interacting = false;
                        movement.characterAnimator.SetBool("ButtonPress", true);

                        yield return new WaitForSeconds(0.5f);

                        player.transform.parent = platform;
                        animator.SetBool("isAscending", true);

                        yield return new WaitForSeconds(2.8f);

                        if (Random.Range(0, 20) == 0) player.GetComponentInChildren<Animator>().SetBool("isDancing", true);
                    }
                    break;
            }
            yield return null;
        }
    }

    private void IncreaseFloor()
    {
        FloorManager.floor++;
        SceneLoader.instance.LoadScene("GameScene");
    }

    public void PlaySound(string name)
    {
        Audio.PlaySound(name, null);
    }
}
