using UnityEngine;

public class SpriteIdler : MonoBehaviour
{
    public DialogueManager dialogueManager;   // Reference to the script that has dialogueActive
    public Sprite idleSprite;                   // The sprite to switch to
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (dialogueManager != null && !dialogueManager.dialogueActive)
        {
            spriteRenderer.sprite = idleSprite;
        }
    }
}
