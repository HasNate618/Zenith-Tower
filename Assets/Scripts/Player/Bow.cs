using UnityEngine;

public class Bow : MonoBehaviour
{
    public Transform leftHand, rightHand, back, bowString, bowRoot;
    private Vector3 bowStringOrigin;

    [SerializeField] Transform arrowPrefab;
    public Transform heldArrow;

    void Start()
    {
        bowStringOrigin = bowString.localPosition;
    }

    public void GrabBow()
    {
        back.gameObject.SetActive(false);
        leftHand.gameObject.SetActive(true);
        DrawBow();
    }

    public void DrawBow()
    {
        //Grab String
        bowString.position = rightHand.position;
        bowString.parent = rightHand;

        //Spawn arrow
        heldArrow = Instantiate(arrowPrefab, rightHand.position, transform.rotation, rightHand);
    }

    public void UndrawBow(float stowTime)
    {
        bowString.parent = bowRoot;
        bowString.localPosition = bowStringOrigin;
        Invoke("StowBow", stowTime);
    }

    public void StowBow()
    {
        back.gameObject.SetActive(true);
        leftHand.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (heldArrow != null)
        {
            heldArrow.rotation = bowRoot.rotation;
            heldArrow.localRotation *= Quaternion.Euler(90, 0, 0);
        }
    }
}
