using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject inventoryText;
    private bool isOpen;
    void Start()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
            inventoryText.SetActive(false);
        }
    }

    public void OnInventory(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        ToggleInventory();
    }

    private void ToggleInventory()
    {
        isOpen = !isOpen;

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(isOpen);
            inventoryText.SetActive(isOpen);
        }
    }

   
}
