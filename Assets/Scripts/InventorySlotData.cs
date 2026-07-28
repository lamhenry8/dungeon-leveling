using UnityEngine;

public class InventorySlotData : MonoBehaviour
{
    public ItemData item;
    public int amount;

    public InventorySlotData(ItemData item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}
