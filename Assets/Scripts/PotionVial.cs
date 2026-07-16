using UnityEngine;

public class PotionVial : InteractableResource
{
    private Potions potionManager;

    void Start()
    {
        resourceName = "Potion Vial";
        useMessage = "Press E to acquire Potion Vial";
        // interactTrigger = "PickFruit";
        potionManager = FindAnyObjectByType<Potions>();
    }

    public override void Interact()
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
