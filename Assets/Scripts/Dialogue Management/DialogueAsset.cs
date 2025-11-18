using UnityEngine;

public enum DialogueEndAction { Close, OpenBranch, RunScript }

[CreateAssetMenu(menuName = "Dialogue/Dialogue")]
public class DialogueAsset : ScriptableObject
{
    [TextArea(2, 5)]
    public string[] lines;

    public CharacterAsset[] characters;

    // For each line, which character index speaks
    public int[] speakingCharacterIndices;

    // Optional: For each character, sprites to update on each line (same length as lines)
    // e.g. 2D array: characters.Length x lines.Length
    public Sprite[,] characterSpritesPerLine;

    // What to do when finished
    public DialogueEndAction onFinish = DialogueEndAction.Close;

    // Optional references for branch or script to run
    public BranchAsset branchToOpen;
    public UnityEngine.Events.UnityEvent onFinishScript;
}
