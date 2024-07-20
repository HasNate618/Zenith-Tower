using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    private Vector3 forward, right;
    [SerializeField] CharacterController characterController;
    [SerializeField] Joystick joystick;
    public float speed = 4f, speedMultiplier = 1;
    [SerializeField] float rotationSpeed = 1;
    public bool isAiming, isRolling;
    private bool isMoving, rollCooldown;

    public GameObject[] joysticks;

    [SerializeField] int rollSpeed;
    [SerializeField] float cooldownTime; 
    private float cooldownTimer;
    [SerializeField] Button rollButton;
    [SerializeField] Image rollFill;

    private int isRunningHash, isRollingHash, buttonPressHash, velocityXHash, velocityZHash;
    public Animator characterAnimator;
    public Transform armature;

    [SerializeField] PlayerHealth health;
    public bool dead;

    void Awake()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        isRunningHash = Animator.StringToHash("isRunning");
        isRollingHash = Animator.StringToHash("isRolling");
        buttonPressHash = Animator.StringToHash("ButtonPress");
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityZHash = Animator.StringToHash("VelocityZ");
    }

    private void Update()
    {
        if (dead)
        {
            return;
        }

        joysticks[0].SetActive(!joysticks[1].activeSelf);
        joysticks[2].SetActive(!joysticks[3].activeSelf);


        if (!isRolling)
        {
            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                if (!isMoving)
                {
                    isMoving = true;
                    characterAnimator.SetBool(isRunningHash, true);
                }

                //Calculate movement
                Vector2 joystickNormalized = new Vector2(joystick.Horizontal, joystick.Vertical).normalized;
                Vector3 rightMovement = right * speed * speedMultiplier * Time.deltaTime * joystickNormalized.x;
                Vector3 upMovement = forward * speed * speedMultiplier * Time.deltaTime * joystickNormalized.y;
                Vector3 movement = rightMovement + upMovement;
                characterController.Move(movement);

                if (!isAiming)
                {
                    //Calculate Rotation
                    Quaternion targetRotation = Quaternion.LookRotation(movement);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

            }
            else if (isMoving)
            {
                isMoving = false;
                characterAnimator.SetBool(isRunningHash, false);
            }

            if (isAiming)
            {
                //Rotate armature to aiming direction
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, CharacterShooting.shootDirection, 0));
                armature.rotation = Quaternion.Slerp(armature.rotation, targetRotation, 25 * Time.deltaTime);
                //armature.rotation = Quaternion.Euler(0, CharacterShooting.shootDirection, 0);

                //Set animation parameters
                Vector3 joystickMovement = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

                float velocityZ = Vector3.Dot(joystickMovement.normalized, armature.transform.forward);
                float velocityX = Vector3.Dot(joystickMovement.normalized, armature.transform.right);

                characterAnimator.SetFloat(velocityZHash, velocityZ, 0.1f, Time.deltaTime);
                characterAnimator.SetFloat(velocityXHash, velocityX, 0.1f, Time.deltaTime);
            }
        } else
        {
            characterController.Move(transform.forward * rollSpeed * Time.deltaTime);
        }

        if (rollCooldown)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer < cooldownTime)
            {
                rollFill.fillAmount = cooldownTimer / cooldownTime;
            }
            else
            {
                rollCooldown = false;
                rollButton.interactable = true;
                rollFill.fillAmount = 1;
            }
        }
    }

    public void StartRoll()
    {
        if (!rollCooldown)
        {
            characterAnimator.SetBool(isRollingHash, true);
            transform.rotation = armature.rotation;
            armature.localRotation = Quaternion.Euler(0, 0, 0);

            health.invincible = true;

            isRolling = true;
            cooldownTimer = 0;
            rollFill.fillAmount = 0;
            rollButton.interactable = false;
            rollCooldown = true;
        }
    }

    public void EndRoll()
    {
        characterAnimator.SetBool(isRollingHash, false);
        isRolling = false;
        health.invincible = false;
    }

    private void OnDisable()
    {
        isMoving = false;
        if (rollButton != null)
        {
            rollButton.interactable = false;
        }
        characterAnimator.SetBool(isRunningHash, false);
        characterAnimator.SetBool(isRollingHash, false);
        characterAnimator.SetBool("isAiming", false);
    }

    private void OnEnable()
    {
        rollButton.interactable = true;
    }
}
