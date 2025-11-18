using UnityEngine;

public enum DialogueEndAction { Close, OpenBranch, RunScript }

[CreateAssetMenu(menuName = "Dialogue/Dialogue")]
public class DialogueAsset : ScriptableObject
{
    [TextArea(2, 5)]
    public string[] lines;

    public CharacterAsset[] characters;

    public int[] speakingCharacterIndices;

    // NEW: For each line, which sprite index to use for that character's emotion
    public int[] emotionSpriteIndices;

    public DialogueEndAction onFinish = DialogueEndAction.Close;

    public BranchAsset branchToOpen;
    public UnityEngine.Events.UnityEvent onFinishScript;
}

