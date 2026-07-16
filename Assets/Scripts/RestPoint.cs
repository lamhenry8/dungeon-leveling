using UnityEngine;

public class RestPoint : InteractableResource
{
    private Potions potionManager;

    void Start()
    {
        resourceName = "Rest Point";
        useMessage = "Press E to Rest";
        interactTrigger = "PickFruit";
        destroyWhenEmpty = false;
        usesRemaining = 999;
        potionManager = FindAnyObjectByType<Potions>();
    }

    public override void Interact()
    {
        if (potionManager != null && potionManager.HasVial())
        {
            potionManager.RefillPotions();
            Debug.Log("Potions refilled at rest point!");
        }
        else if (potionManager != null)
        {
            Debug.Log("You need a Potion Vial to use rest points.");
        }
    }
}
