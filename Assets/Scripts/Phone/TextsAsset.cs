using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewTexts", menuName = "Phone/Texts")]
public class TextsAsset : ScriptableObject
{
    [TextArea(2, 10)]
    public List<string> messages = new List<string>();

    public void AddMessage(string message)
    {
        messages.Add(message);
    }

    public void ClearMessages()
    {
        messages.Clear();
    }
}
