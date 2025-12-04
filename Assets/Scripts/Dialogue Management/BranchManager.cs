using UnityEngine;
using UnityEngine.UI;

public class BranchManager : MonoBehaviour
{
    public static BranchManager Instance;

    public GameObject branchPanel;
    public Button choice1Button;
    public Button choice2Button;

    private BranchAsset currentBranch;

    [Header("Phone Shit")]
    public GameObject phonePanel;

    [Header("Player Movement")]
    public PlayerMovement playerMovement;

    public bool branchActive { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        choice1Button.onClick.AddListener(() => OnChoiceSelected(1));
        choice2Button.onClick.AddListener(() => OnChoiceSelected(2));
    }

    public void StartBranch(BranchAsset branch)
    {
        currentBranch = branch;
        branchPanel.SetActive(true);
        phonePanel.SetActive(false);
        branchActive = true;

        playerMovement.canMove = false;
        playerMovement.canLook = false;

        choice1Button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = branch.choice1Text;
        choice2Button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = branch.choice2Text;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnChoiceSelected(int choice)
    {
        branchPanel.SetActive(false);
        branchActive = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerMovement.canMove = false;
        playerMovement.canLook = false;

        DialogueAsset nextDialogue = choice == 1 ? currentBranch.choice1Dialogue : currentBranch.choice2Dialogue;

        if (nextDialogue != null)
            DialogueManager.Instance.StartDialogue(nextDialogue);
    }

    public void ForceCloseBranchSafe()
    {
        if (!branchActive)
            return; // nothing to close

        // Hide branch UI
        branchPanel.SetActive(false);

        // Mark branch as inactive
        branchActive = false;

        // Restore player control
        playerMovement.canMove = true;
        playerMovement.canLook = true;

        // Lock and hide cursor for gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Branch force-closed safely.");
    }
}
