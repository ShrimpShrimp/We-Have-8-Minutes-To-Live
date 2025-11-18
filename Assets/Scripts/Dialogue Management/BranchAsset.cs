using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Branch")]
public class BranchAsset : ScriptableObject
{
    public string choice1Text;
    public DialogueAsset choice1Dialogue;

    public string choice2Text;
    public DialogueAsset choice2Dialogue;
}
