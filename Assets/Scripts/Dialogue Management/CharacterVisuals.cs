using UnityEngine;

public class CharacterVisuals : MonoBehaviour
{
    public string characterId; // must match the CharacterAsset.characterId

    public SpriteRenderer spriteRenderer;
    public Sprite[] emotionSprites;

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
