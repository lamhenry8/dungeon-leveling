using UnityEngine;

public class InteractableResource : MonoBehaviour
{
    public ItemData item;
    public int amountPerCollect = 1;
    public int usesRemaining = 1;
    
    public string useMessage = "Press E to Interact";
    public string interactTrigger = "PickFruit";
    public bool destroyWhenEmpty = true;



    public virtual void Interact(Inventory inventory)
    {
        if (usesRemaining <= 0)
        {
            return;
        }

        if (item != null && inventory != null)
        {
            inventory.AddItem(item, amountPerCollect);
        }

        usesRemaining--;

        if (usesRemaining <= 0 && destroyWhenEmpty)
        {
            gameObject.SetActive(false);
        }
    }
}
