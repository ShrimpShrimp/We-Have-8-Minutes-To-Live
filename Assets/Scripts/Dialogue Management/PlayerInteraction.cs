using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI interactPrompt;

    [Header("Settings")]
    public float interactDistance = 3f;   // How far you can interact

    public LayerMask interactableLayer;   // Assign the layer(s) your interactables are on

    private Interactable currentInteractable;

    public static PlayerInteraction Instance;
    public DialogueManager dialogue;
    public BranchManager branch;

    public bool canInteract = true;

    private Camera playerCamera;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        playerCamera = Camera.main;

        if (interactPrompt != null)
            // interactPrompt.enabled = false; // Hide initially
            interactPrompt.gameObject.SetActive(false); 
    }

    public void SetCanInteract(bool state)
    {
        canInteract = state;

        if (!canInteract && interactPrompt != null)
            // interactPrompt.enabled = false; // Hide prompt if can't interact
            interactPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!canInteract || !dialogue.CanStartDialogue() || branch.branchActive)
        {
            currentInteractable = null;
            if (interactPrompt != null)
                // interactPrompt.enabled = false;
                interactPrompt.gameObject.SetActive(false);
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                if (interactable != currentInteractable)
                {
                    currentInteractable = interactable;
                    if (interactPrompt != null)
                        interactPrompt.gameObject.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentInteractable.Interact();
                }
            }
            else
            {
                ClearInteractable();
            }
        }
        else
        {
            ClearInteractable();
        }
    }

    private void ClearInteractable()
    {
        currentInteractable = null;
        if (interactPrompt != null)
            // interactPrompt.enabled = false;
            interactPrompt.gameObject.SetActive(false);
    }
}
