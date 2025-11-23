using UnityEngine;
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

    [Header("Player")]
    public PlayerMovement playerMovement;

    [Header("Character Visuals")]
    public CharacterVisualsManager visualsManager; // Assign in inspector


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
            if (typingCoroutine != null)
            {
                // Optionally: play a 'cannot skip' sound or ignore
                return;
            }

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

        int speakerIndex = currentDialogue.speakingCharacterIndices[currentLineIndex];
        CharacterAsset speaker = currentDialogue.characters[speakerIndex];

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

        // Get visuals by characterId via the visualsManager
        if (visualsManager != null)
        {
            CharacterVisuals visuals = visualsManager.GetVisualsById(speaker.characterId);
            if (visuals != null)
            {
                int emotionIndex = speaker.defaultSpriteIndex; // fallback default

                if (currentDialogue.emotionSpriteIndices != null &&
                    currentDialogue.emotionSpriteIndices.Length > currentLineIndex)
                {
                    emotionIndex = currentDialogue.emotionSpriteIndices[currentLineIndex];
                }

                visuals.SetEmotionSprite(emotionIndex);
            }
            else
            {
                Debug.LogWarning($"No CharacterVisuals found for characterId '{speaker.characterId}'.");
            }

            if (playerMovement != null)
            {
                Transform cameraTarget = visualsManager.GetCameraTargetById(speaker.characterId);

                if (cameraTarget != null)
                    playerMovement.RotateCameraToTarget(cameraTarget.transform);
                else
                    Debug.LogWarning($"No camera target assigned for speaker '{speaker.characterId}'.");
            }
        }
        else
        {
            Debug.LogWarning("VisualsManager reference is not assigned in DialogueManager.");
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
        audioSource.Stop();
    }

    private void NextLine()
    {
        currentLineIndex++;

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

        dialogueClosedTime = Time.time;

        

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

            case DialogueEndAction.TriggerEvent:
                if (!string.IsNullOrEmpty(currentDialogue.eventId))
                {
                    Debug.Log($"Triggering event: {currentDialogue.eventId}");
                    EventManager.TriggerEvent(currentDialogue.eventId);
                }
                else
                {
                    Debug.LogWarning("Dialogue set to TriggerEvent but no eventId specified!");
                }
                break;
        }
    }

}
