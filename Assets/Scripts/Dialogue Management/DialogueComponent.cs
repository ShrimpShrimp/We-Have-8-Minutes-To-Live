using UnityEngine;

public class DialogueComponent : Interactable
{
    public DialogueAsset currentDialogue;

    private float interactCooldown = 0.5f;
    private float lastInteractTime = -10f;

    public override void Interact()
    {
        Debug.Log("Interacted");
        if (Time.time - lastInteractTime < interactCooldown)
            return;

        if (!DialogueManager.Instance.dialogueActive && !BranchManager.Instance.branchActive)
        {
            DialogueManager.Instance.StartDialogue(currentDialogue);
            lastInteractTime = Time.time;
        }
    }
}