using UnityEngine;

public class CharacterVisuals : MonoBehaviour
{
    public string characterId; // must match CharacterAsset.characterId

    [Header("Sprite Rendering")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] emotionSprites;

    [Header("Camera Target")]
    public Transform cameraTarget;
    // Assign in Inspector — usually the same object, or an empty child for better framing

    public void SetEmotionSprite(int index)
    {
        if (emotionSprites == null || emotionSprites.Length == 0)
        {
            Debug.LogWarning("No sprites assigned.");
            return;
        }

        if (index < 0 || index >= emotionSprites.Length)
        {
            Debug.LogWarning($"Emotion index {index} out of range.");
            return;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = emotionSprites[index];
        }
    }
}
