using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "NewContact", menuName = "Phone/Contact")]
public class ContactAsset : ScriptableObject
{
    public string contactName;
    public TextsAsset textsAsset;

    // A delegate or method reference to PhoneManager's call methods
    public System.Action callAction;

    public void Call()
    {
        if (callAction != null)
            callAction.Invoke();
        else
            Debug.LogWarning($"No call action assigned for {contactName}");
    }
}

