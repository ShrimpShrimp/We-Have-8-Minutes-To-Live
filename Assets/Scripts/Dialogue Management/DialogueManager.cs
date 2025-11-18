using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Typing")]
    public float typingSpeed = 0.05f;

    private DialogueAsset currentDialogue;
    private int currentLineIndex = 0;

    private Coroutine typingCoroutine;

    private AudioSource audioSource;

    public bool dialogueActive { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!dialogueActive)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            // If typing is still happening, ignore input (do nothing)
            if (typingCoroutine != null)
            {
                // You can optionally play a "cannot skip" sound here or just ignore
                return;
            }

            // If typing is done, move to next line
            NextLine();
        }
    }

    public void StartDialogue(DialogueAsset dialogue)
    {
        Debug.Log("Dialogue started.");
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialogueActive = true;
        dialoguePanel.SetActive(true);
        ShowLine();
    }

    private void ShowLine()
    {
        if (currentLineIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        CharacterAsset speaker = currentDialogue.characters[currentDialogue.speakingCharacterIndices[currentLineIndex]];

        // Set text color
        dialogueText.color = speaker.textColor;


        // Play typing sound loop
        if (speaker.typingSound != null)
        {
            audioSource.clip = speaker.typingSound;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }

        typingCoroutine = StartCoroutine(TypeText(currentDialogue.lines[currentLineIndex]));
    }

    private IEnumerator TypeText(string line)
    {
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
        // Stop typing sound when done
        audioSource.Stop();
    }

    private void NextLine()
    {
        currentLineIndex++;

        // You can update character sprites per line here if you implement that feature

        if (currentLineIndex < currentDialogue.lines.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

  

    private float dialogueCooldown = 0.3f;
    private float dialogueClosedTime = -10f;

    public bool CanStartDialogue()
    {
        return Time.time - dialogueClosedTime > dialogueCooldown && !dialogueActive && !BranchManager.Instance.branchActive;
    }

    private void EndDialogue()
    {

        Debug.Log("Dialogue ended.");
        dialogueActive = false;
        dialoguePanel.SetActive(false);
        audioSource.Stop();

        dialogueClosedTime = Time.time;  // mark when dialogue closed

        switch (currentDialogue.onFinish)
        {
            case DialogueEndAction.Close:
                break;

            case DialogueEndAction.OpenBranch:
                if (currentDialogue.branchToOpen != null)
                    BranchManager.Instance.StartBranch(currentDialogue.branchToOpen);
                break;

            case DialogueEndAction.RunScript:
                currentDialogue.onFinishScript?.Invoke();
                break;
        }
    }
}
