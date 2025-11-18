using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Character")]
public class CharacterAsset : ScriptableObject
{
    public string characterId; // e.g. "bob", "alice"
    public string characterName; // human-readable name
    public Color textColor;
    public AudioClip typingSound;

    public int scaredSpriteIndex = 0;
    public int angrySpriteIndex = 1;
    public int happySpriteIndex = 2;
    public int sadSpriteIndex = 3;
    public int talkingSpriteIndex = 4;
}

