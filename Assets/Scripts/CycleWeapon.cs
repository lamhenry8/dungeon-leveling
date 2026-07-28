using UnityEngine;
using UnityEngine.InputSystem;

public class CycleWeapon : MonoBehaviour
{
    [SerializeField] private GameObject[] equipmentSlots = new GameObject[3];

    private int currentIndex = -1;

    void Start()
    {
        SetActiveSlot(currentIndex);
    }

    public void OnCycleLeft(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        Cycle(-1);
    }

    public void OnCycleRight(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        Cycle(1);
    }

    private void Cycle(int direction)
    {
        int slotCount = equipmentSlots.Length;
        if (slotCount == 0)
        {
            return;
        }

        int nextIndex = currentIndex;

        for (int i = 0; i < slotCount; i++)
        {
            nextIndex = (nextIndex + direction + slotCount) % slotCount;

            if (equipmentSlots[nextIndex] != null)
            {
                break;
            }
        }

        SetActiveSlot(nextIndex);
    }

    private void SetActiveSlot(int index)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i] != null)
            {
                equipmentSlots[i].SetActive(i == index);
            }
        }

        currentIndex = index;
    }
}