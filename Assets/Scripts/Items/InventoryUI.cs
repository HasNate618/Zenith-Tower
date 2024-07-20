using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Color[] rarityColour;
    [SerializeField] GameObject inventoryItem;
    [SerializeField] GameObject batteryItem;

    public Transform[] inventorySlots;
    public Transform[] bowSlots;
    public Transform[] suitSlots;

    public Text itemDesc;

    private void Start()
    {
        StartCoroutine(OnUpdateInventory(inventorySlots));
        StartCoroutine(OnUpdateInventory(bowSlots));
        StartCoroutine(OnUpdateInventory(suitSlots));

        if (EquipmentSlot.enhancements == null) EquipmentSlot.enhancements = GameObject.FindGameObjectWithTag("Player").GetComponent<Enhancements>();
        if (EquipmentSlot.ui == null) EquipmentSlot.ui = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryUI>();

        foreach (Transform itemSlot in bowSlots)
        {
            itemSlot.GetComponent<EquipmentSlot>().Invoke("CheckIfUsed", 0.1f);
        }

        foreach (Transform itemSlot in suitSlots)
        {
            itemSlot.GetComponent<EquipmentSlot>().Invoke("CheckIfUsed", 0.1f);
        }
    }

    public IEnumerator OnUpdateInventory(Transform[] slotGroup)
    {
        //Clear Inventory
        foreach (Transform slot in inventorySlots)
        {
            if (slot.childCount > 0)
            {
                GameObject child = slot.GetComponentsInChildren<Transform>()[1].gameObject;
                Destroy(child);
            }
            else break;
        }        
        foreach (Transform slot in bowSlots)
        {
            if (slot.childCount > 0)
            {
                GameObject child = slot.GetComponentsInChildren<Transform>()[1].gameObject;
                Destroy(child);
            }
            else break;
        }        
        foreach (Transform slot in suitSlots)
        {
            if (slot.childCount > 0)
            {
                GameObject child = slot.GetComponentsInChildren<Transform>()[1].gameObject;
                Destroy(child);
            }
            else break;
        }


        //Draws inventory
        yield return null;

        int i = 0;
        foreach (InventoryItemData item in InventorySystem.inventory) AddItemToSlot(item, inventorySlots, i++, 1);
        i = 0;
        foreach (InventoryItemData item in InventorySystem.equippedBowEnhancements) AddItemToSlot(item, bowSlots, i++, 2);
        i = 0;
        foreach (InventoryItemData item in InventorySystem.equippedSuitEnhancements) AddItemToSlot(item, suitSlots, i++, 3);
    }

    public void AddItemToSlot(InventoryItemData itemData, Transform[] slotGroup, int slot, int inventoryType)
    {
        if (slotGroup[slot].childCount > 0) return;

        if (itemData.id != "Battery")
        {
            GameObject newItem = Instantiate(inventoryItem);
            newItem.transform.SetParent(slotGroup[slot], false);

            newItem.GetComponent<Image>().color = rarityColour[itemData.level - 1];
            newItem.GetComponentsInChildren<Image>()[1].sprite = itemData.icon;
            newItem.GetComponent<InventoryItemUI>().referenceData = itemData;

            newItem.GetComponent<InventoryItemUI>().inventoryType = inventoryType;
        }
        else
        {
            GameObject newItem = Instantiate(batteryItem);
            newItem.transform.SetParent(slotGroup[slot], false);
        }
    }

}
