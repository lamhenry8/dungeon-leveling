using UnityEngine;
using System.Collections;

public class Sword : InteractableResource
{
    [SerializeField] private Transform handSocket;
    [SerializeField] private Vector3 finalPosition = Vector3.zero;
    [SerializeField] private Vector3 finalRotation = Vector3.zero;
    [SerializeField] private string pickupAnimationTrigger = "PickUp";
    [SerializeField] private float pickupDelay = 0.5f;

    private bool isPickedUp = false;
    private Animator animator;

    void Start()
    {
        if (handSocket == null)
        {
            Animator playerAnimator = FindAnyObjectByType<Animator>();
            if (playerAnimator != null)
            {
                handSocket = playerAnimator.GetBoneTransform(HumanBodyBones.RightHand);
            }
        }

        animator = FindAnyObjectByType<Animator>();
    }

    public override void Interact()
    {
        if (isPickedUp)
        {
            return;
        }

        isPickedUp = true;
        StartCoroutine(PickupRoutine());
    }

    private IEnumerator PickupRoutine()
    {
        if (animator != null && !string.IsNullOrEmpty(pickupAnimationTrigger))
        {
            animator.SetTrigger(pickupAnimationTrigger);
        }

        yield return new WaitForSeconds(pickupDelay);

        transform.SetParent(handSocket);
        transform.SetLocalPositionAndRotation(finalPosition, Quaternion.Euler(finalRotation));
        GetComponent<Collider>().enabled = false;
    }
}
