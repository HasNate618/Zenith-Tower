using System.Collections;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
    [SerializeField] CharacterMovement movement;
    [SerializeField] Bow bow;

    [SerializeField] Transform hand;
    [SerializeField] Joystick joystick;
    [SerializeField] GameObject trajectoryAnchor;
    [SerializeField] Transform trajectoryLine;
    [SerializeField] Material trajectoryMat;
    [SerializeField] Color defaultTrajectoryColour;
    [SerializeField] Color hitTrajectoryColour;
    public static float shootDirection;
    private static bool inverted;
    private bool isAiming;

    [SerializeField] Transform cameraTarget;

    private float _bowRange = 6;
    public float bowRange
    {
        get
        {
            return _bowRange;
        }
        set
        {
            _bowRange = value;
            trajectoryLine.localScale = new Vector3(0.1f, value, 1);
            trajectoryLine.localPosition = new Vector3(0, -1.59f, value / 2);
        }
    }

    public float arrowDamage, reloadSpeed;

    public bool dead, isReloading;

    [SerializeField] Color arrowColour;
    [SerializeField] Material arrowMat;
    [SerializeField] SlicedFilledImage[] ammoBars;
    [SerializeField] Color availableShotColour;
    [SerializeField] Color unavailableShotColour;
    private int availableShots = 3;

    private void Start()
    {
        trajectoryMat.color = defaultTrajectoryColour;
        if (FloorManager.floor == 0) inverted = Settings.invertedShooting;
    }

    public void ShootArrow()
    {
        if (availableShots > 0)
        {
            //Shoot Arrow
            bow.heldArrow.GetComponent<Projectile>().Setup(shootDirection, _bowRange, new Vector3(transform.position.x, hand.position.y, transform.position.z), arrowDamage, GetComponent<PlayerHealth>(), "Enemy");
            bow.heldArrow.parent = null;
            bow.heldArrow = null;

            if (availableShots < 3) ammoBars[availableShots].fillAmount = 0;
            availableShots--;
            ammoBars[availableShots].color = unavailableShotColour;
        }
        else
        {
            //Dont Shoot
            Destroy(bow.heldArrow.gameObject);
        }
    }

    public void CancelAiming()
    {
        if (isAiming)
        {
            isAiming = false;
            trajectoryAnchor.SetActive(false);
            Destroy(bow.heldArrow.gameObject);
            bow.UndrawBow(0);
            cameraTarget.localPosition = Vector3.zero;

            movement.isAiming = false;
            movement.speedMultiplier = 1;
            movement.characterAnimator.SetBool("isAiming", false);
        }
    }

    public void StartAiming()
    {
        isAiming = true;
        trajectoryAnchor.SetActive(true);
        movement.isAiming = true;
        movement.speedMultiplier = 0.3f;
        movement.characterAnimator.SetBool("isAiming", true);
        cameraTarget.localPosition = new Vector3(0, 0, 1);

        bow.GrabBow();
        bow.CancelInvoke();
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        float elapsed = 0;

        while (elapsed < reloadSpeed)
        {
            elapsed += Time.deltaTime;
            ammoBars[availableShots].fillAmount = elapsed / reloadSpeed;
            yield return null;
        }

        ammoBars[availableShots].color = availableShotColour;
        availableShots++;
        isReloading = false;
    }

    void Update()
    {
        if (availableShots < 3 && !isReloading) StartCoroutine(Reload());

        if (movement.isRolling || dead)
        {
            CancelAiming();
            return;
        }

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            if (!isAiming) StartAiming();

            shootDirection = (Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * Mathf.Rad2Deg) - 30;

            if (inverted) shootDirection += 180;

            trajectoryAnchor.transform.rotation = Quaternion.Euler(0, shootDirection, 0);

            if (Physics.Raycast(trajectoryAnchor.transform.position, trajectoryAnchor.transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, _bowRange))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    trajectoryAnchor.transform.localScale = new Vector3(1, 1, hitInfo.distance / _bowRange);
                }
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    trajectoryAnchor.transform.localScale = new Vector3(1, 1, hitInfo.distance / _bowRange);
                    trajectoryMat.color = hitTrajectoryColour;
                }
                else trajectoryMat.color = defaultTrajectoryColour;
            }
            else if (trajectoryAnchor.transform.localScale.z < 1)
            {
                trajectoryAnchor.transform.localScale = new Vector3(1, 1, 1);
                trajectoryMat.color = defaultTrajectoryColour;
            }
        }
        else if (isAiming)
        {
            //Stops Aiming

            isAiming = false;
            trajectoryAnchor.SetActive(false);
            ShootArrow();
            movement.isAiming = false;
            movement.speedMultiplier = 1;
            movement.characterAnimator.SetBool("isAiming", false);

            transform.rotation = movement.armature.rotation;
            movement.armature.localRotation = Quaternion.Euler(0, 0, 0);

            cameraTarget.localPosition = new Vector3(0, 0, 0);

            bow.UndrawBow(0);
        }
    }
}
