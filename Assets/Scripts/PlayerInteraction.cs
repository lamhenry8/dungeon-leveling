using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 5f;
    [SerializeField] private TextMeshProUGUI promptText;

    private InteractableResource currentResource;
    private Animator animator;
    private bool isInteracting;
    private readonly WaitForSeconds interactDelay = new(0.5f);
    private readonly Collider[] overlapResults = new Collider[50];

    void Start()
    {
        animator = GetComponent<Animator>();

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        FindNearbyResource();
    }

    private void FindNearbyResource()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, interactionRange, overlapResults);

        InteractableResource closestResource = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < count; i++)
        {
            Collider collider = overlapResults[i];
            InteractableResource resource = collider.GetComponent<InteractableResource>();

            if (resource == null)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, collider.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestResource = resource;
            }
        }

        currentResource = closestResource;

        if (promptText == null)
        {
            return;
        }

        if (currentResource != null && !isInteracting)
        {
            promptText.text = currentResource.useMessage;
            promptText.gameObject.SetActive(true);
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
    }

    public void OnInteract(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        if (currentResource == null)
        {
            return;
        }

        if (isInteracting)
        {
            return;
        }

        StartCoroutine(InteractRoutine());
    }

    private IEnumerator InteractRoutine()
    {
        isInteracting = true;

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }

        if (animator != null && !string.IsNullOrEmpty(currentResource.interactTrigger))
        {
            animator.SetTrigger(currentResource.interactTrigger);
        }

        yield return interactDelay;

        if (currentResource != null)
        {
            currentResource.Interact();
        }

        yield return interactDelay;

        isInteracting = false;
    }
}
