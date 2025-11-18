using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Character")]
public class CharacterAsset : ScriptableObject
{
    public Color textColor = Color.white;
    public Sprite characterSprite;    // Sprite to show on dialogue or character portrait
    public AudioClip typingSound;
}
