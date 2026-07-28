using UnityEngine;

public class PotionVial : InteractableResource
{
    private Potions potionManager;

    void Start()
    {
        useMessage = "Press E to acquire Potion Vial";
        potionManager = FindAnyObjectByType<Potions>();
    }

    public override void Interact(Inventory inventory)
    {
        if (usesRemaining <= 0)
        {
            return;
        }

        if (potionManager != null)
        {
            potionManager.AcquireVial();
        }

        usesRemaining--;

        if (usesRemaining <= 0 && destroyWhenEmpty)
        {
            gameObject.SetActive(false);
        }
    }
}
