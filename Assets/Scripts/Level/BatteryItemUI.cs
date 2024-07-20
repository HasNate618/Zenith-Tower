using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BatteryItemUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public InventoryItemData referenceData;

    [SerializeField] CanvasGroup group;
    [SerializeField] RectTransform rectTransform;

    private Text itemDesc;
    [SerializeField] string batteryDesc;

    [SerializeField] Transform fabricationSlot;
    [SerializeField] InventoryItemData batteryReference;
    private Transform player;

    private void Awake()
    {
        itemDesc = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryUI>().itemDesc;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        fabricationSlot = player.GetComponent<FabricationManager>().fabricationSlot;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (fabricationSlot.GetComponentInChildren<BatteryItemUI>() == this)
        {
            if (InventorySystem.Add(batteryReference))
            {
                Destroy(gameObject);
                player.GetComponent<PlayerHealth>().UpdateBatteryCount();
            }
        }
        else
        {
            itemDesc.text = referenceData.description;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        itemDesc.text = null;
    }
}
