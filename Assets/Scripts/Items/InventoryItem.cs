public class InventoryItem
{
    public InventoryItemData data { get; private set; }

    public InventoryItem(InventoryItemData source)
    {
        data = source;
    }
}
