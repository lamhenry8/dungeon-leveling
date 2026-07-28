using UnityEngine;

public class DoorLever : InteractableResource
{
    [SerializeField] private DoorOpen door;

    void Awake()
    {
        useMessage = "Press E to Open Door";
        interactTrigger = "";
        destroyWhenEmpty = false;
        usesRemaining = 999;
    }

    public override void Interact(Inventory inventory)
    {
        if (door != null)
        {
            door.Open();
        }
    }
}