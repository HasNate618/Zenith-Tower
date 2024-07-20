using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    static public List<InventoryItemData> inventory { get; private set; } = new List<InventoryItemData>();
    static public List<InventoryItemData> equippedBowEnhancements { get; private set; } = new List<InventoryItemData>();
    static public List<InventoryItemData> equippedSuitEnhancements { get; private set; } = new List<InventoryItemData>();

    private static InventoryUI ui;

    private void Awake()
    {
        if (FloorManager.floor == 0)
        {
            inventory.Clear();
            equippedBowEnhancements.Clear();
            equippedSuitEnhancements.Clear();
        }
        ui = GetComponent<InventoryUI>();
    }

    //Debugging items in inventory
    /*[SerializeField] List<InventoryItemData> inv = new List<InventoryItemData>();
    [SerializeField] List<InventoryItemData> bow = new List<InventoryItemData>();
    [SerializeField] List<InventoryItemData> suit = new List<InventoryItemData>();
    private void Update()
    {
        inv = inventory;
        bow = equippedBowEnhancements;
        suit = equippedSuitEnhancements;
    }*/

    public static bool Add(InventoryItemData referenceData)
    {
        if (inventory.Count < ui.inventorySlots.Length)
        {
            inventory.Add(referenceData);

            ui.StartCoroutine(ui.OnUpdateInventory(ui.inventorySlots));
            return true;
        }
        return false;
    }

    public static void Remove(InventoryItemData referenceData)
    {
        if (inventory.Contains(referenceData))
        {
            inventory.Remove(referenceData);
            ui.StartCoroutine(ui.OnUpdateInventory(ui.inventorySlots));
        }
    }

    public static void Reset()
    {
        print("Inventory Reset");
    }
}
