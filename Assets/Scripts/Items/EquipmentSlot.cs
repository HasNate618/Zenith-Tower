using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private int slotType;
    public static Enhancements enhancements;
    public static InventoryUI ui;

    public void CheckIfUsed()
    {
        if (transform.childCount > 0)
        {
            ApplyEnhancement(GetComponentInChildren<InventoryItemUI>().referenceData, false);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && InventoryItemUI.selectedItem != null)
        {
            InventoryItemUI selectedItem = InventoryItemUI.selectedItem.GetComponent<InventoryItemUI>();

            if (selectedItem.inventoryType == slotType) return;
            ApplyEnhancement(selectedItem.referenceData, true);
        }
    }

    private void ApplyEnhancement(InventoryItemData data, bool updateInventory)
    {
        if (data == null)
        {
            Debug.LogError("Reference Data to add is NULL");
            return;
        }

        switch (slotType)
        {
            case 1:
                if (ui.inventorySlots.Length == InventorySystem.inventory.Count) return;

                switch (data.id)
                {
                    case "Enhancements_Damage":
                        enhancements.bowDamageLevel = 0;
                        break;
                    case "Enhancements_Range":
                        enhancements.bowRangeLevel = 0;
                        break;
                    case "Enhancements_Reload":
                        enhancements.reloadSpeedLevel = 0;
                        break;
                    case "Enhancements_Health":
                        enhancements.maxHealthLevel = 0;
                        break;
                    case "Enhancements_Absorption":
                        enhancements.absorptionLevel = 0;
                        break;
                    case "Enhancements_Speed":
                        enhancements.characterSpeedLevel = 0;
                        break;
                    case "Enhancements_Vision":
                        enhancements.visionLevel = 0;
                        break;
                }

                if (updateInventory)
                {
                    InventorySystem.inventory.Add(data);
                    if (InventorySystem.equippedBowEnhancements.Contains(data)) InventorySystem.equippedBowEnhancements.Remove(data);
                    else InventorySystem.equippedSuitEnhancements.Remove(data);
                    ui.StartCoroutine(ui.OnUpdateInventory(ui.inventorySlots));
                }
                break;
            case 2:
                if (updateInventory)
                {
                    if (ui.bowSlots.Length == InventorySystem.equippedBowEnhancements.Count) return;

                    foreach (InventoryItemData itemData in InventorySystem.equippedBowEnhancements)
                    {
                        if (data.id == itemData.id && updateInventory) return;
                    }
                }

                switch (data.id)
                {
                    case "Enhancements_Damage":
                        enhancements.bowDamageLevel = data.level;
                        break;
                    case "Enhancements_Range":
                        enhancements.bowRangeLevel = data.level;
                        break;
                    case "Enhancements_Reload":
                        enhancements.reloadSpeedLevel = data.level;
                        break;
                    case "Enhancements_Health":
                        return;
                    case "Enhancements_Absorption":
                        return;
                    case "Enhancements_Speed":
                        return;
                    case "Enhancements_Vision":
                        return;
                }
                print("4");
                if (updateInventory)
                {
                    InventorySystem.inventory.Remove(data);
                    InventorySystem.equippedBowEnhancements.Add(data);
                    ui.StartCoroutine(ui.OnUpdateInventory(ui.bowSlots));
                    Audio.PlaySound("ApplyEnhancement", null);
                }
                break;
            case 3:
                if (updateInventory)
                {
                    if (ui.suitSlots.Length == InventorySystem.equippedSuitEnhancements.Count) return;

                    foreach (InventoryItemData itemData in InventorySystem.equippedSuitEnhancements)
                    {
                        if (data.id == itemData.id && updateInventory) return;
                    }
                }

                switch (data.id)
                {
                    case "Enhancements_Damage":
                        return;
                    case "Enhancements_Range":
                        return;
                    case "Enhancements_Reload":
                        return;
                    case "Enhancements_Health":
                        enhancements.maxHealthLevel = data.level;
                        break;
                    case "Enhancements_Absorption":
                        enhancements.absorptionLevel = data.level;
                        break;
                    case "Enhancements_Speed":
                        enhancements.characterSpeedLevel = data.level;
                        break;
                    case "Enhancements_Vision":
                        enhancements.visionLevel = data.level;
                        break;
                }
                if (updateInventory)
                {
                    InventorySystem.inventory.Remove(data);
                    InventorySystem.equippedSuitEnhancements.Add(data);
                    ui.StartCoroutine(ui.OnUpdateInventory(ui.suitSlots));
                    Audio.PlaySound("ApplyEnhancement", null);
                }
                break;
            case 4:
                if (updateInventory)
                {
                    InventorySystem.inventory.Remove(data);
                    ui.StartCoroutine(ui.OnUpdateInventory(ui.inventorySlots));

                    CreditsManager.attemptCredits += CreditsManager.instance.itemSellValue[data.level - 1];
                    CreditsManager.instance.UpdateCreditUI();
                    Audio.PlaySound("SciFiAlert", null);
                }
                break;
        }
    }
}
