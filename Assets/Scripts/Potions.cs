using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerHealth))]
public class Potions : MonoBehaviour
{
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private float healAmount = 30f;
    [SerializeField] private TextMeshProUGUI potionChargesText;

    private int currentCharges;
    private bool hasVial;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        currentCharges = 0;
        hasVial = false;
        UpdatePotionUI();
    }

    void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            UsePotion();
        }
    }

    public void UsePotion()
    {
        if (!hasVial)
        {
            Debug.Log("You don't have a Potion Vial yet!");
            return;
        }

        if (currentCharges <= 0)
        {
            Debug.Log("No potion charges remaining!");
            return;
        }

        if (playerHealth.IsAtMaxHealth())
        {
            Debug.Log("Already at full health!");
            return;
        }

        currentCharges--;
        playerHealth.Heal(healAmount);

        UpdatePotionUI();
        Debug.Log($"Used potion! Charges remaining: {currentCharges}");
    }

    public void AcquireVial()
    {
        if (!hasVial)
        {
            hasVial = true;
            Debug.Log("You acquired a Potion Vial!");
            UpdatePotionUI();
        }
    }

    public void RefillPotions()
    {
        if (!hasVial)
        {
            return;
        }

        currentCharges = maxCharges;
        Debug.Log("Potions refilled!");
        UpdatePotionUI();
    }

    private void UpdatePotionUI()
    {
        if (potionChargesText != null)
        {
            if (hasVial)
            {
                potionChargesText.text = $"Potions: {currentCharges}/{maxCharges}";
            }
            else
            {
                potionChargesText.text = "Potions: Locked";
            }
        }
    }

    public bool HasVial() => hasVial;
    public int GetCurrentCharges() => currentCharges;
    public int GetMaxCharges() => maxCharges;
}
