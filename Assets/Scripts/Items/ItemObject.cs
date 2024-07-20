using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;
    private InventoryItemData[][] enhancementsRarity = new InventoryItemData[4][];
    [SerializeField] InventoryItemData[] commonEnhancements, uncommonEnhancements, rareEnhancements, legendaryEnhancements;
    [SerializeField] Material[] material;
    [SerializeField] Image minimapImage;

    private void Awake()
    {
        enhancementsRarity[0] = commonEnhancements;
        enhancementsRarity[1] = uncommonEnhancements;
        enhancementsRarity[2] = rareEnhancements;
        enhancementsRarity[3] = legendaryEnhancements;

        int rarity;
        if (FloorManager.floor > 3) rarity = 3;
        else rarity = FloorManager.floor;
        int item = Random.Range(0, 7);
        referenceItem = enhancementsRarity[rarity][item];
        GetComponentInChildren<MeshRenderer>().material = material[rarity];
        minimapImage.color = material[rarity].GetColor("Base_Colour");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickupItem();
        }
    }

    public void OnPickupItem()
    {
        if (InventorySystem.Add(referenceItem))
        {
            Audio.PlaySound("ItemPickup", null);
            Destroy(gameObject);
        }
    }
}
