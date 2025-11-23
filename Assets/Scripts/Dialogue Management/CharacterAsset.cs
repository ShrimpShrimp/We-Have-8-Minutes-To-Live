using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Character")]
public class CharacterAsset : ScriptableObject
{
    public string characterId; // e.g. "bob", "alice"
    public string characterName; // human-readable name
    public Color textColor;
    public AudioClip typingSound;

    public int defaultSpriteIndex = 0;
    public int defaultAltSpriteIndex = 1;
    public int happySpriteIndex = 2;
    public int laughSpriteIndex = 3;
    public int bitterSweetSpriteIndex = 4;
    public int sadSpriteIndex = 5;
    public int worriedSpriteIndex = 6;
    public int shockedSpriteIndex = 7;
    public int scaredSpriteIndex = 8;
    public int angrySpriteIndex = 9;
}

