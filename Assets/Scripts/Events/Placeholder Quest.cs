using UnityEngine;

public class PlaceholderQuest : MonoBehaviour
{

    public GameObject pillar;
    public DialogueComponent switchComponent;
    public DialogueAsset placeHolderDialogueTwo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.StartListening("PLACEHOLDER_THREE", PlaceHolderThree);
        EventManager.StartListening("PLACEHOLDER_TWO", PlaceHolderTwo);
        EventManager.StartListening("BUILD_PILLAR", BuildPillar);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        EventManager.StopListening("PLACEHOLDER_THREE", PlaceHolderThree);
        EventManager.StopListening("PLACEHOLDER_TWO", PlaceHolderTwo);
        EventManager.StopListening("BUILD_PILLAR", BuildPillar);
    }

    private void PlaceHolderThree()
    {
        Debug.Log("Called placeholder three.");
    }

    private void PlaceHolderTwo()
    {
        Debug.Log("Called placeholder two.");
    }

    private void BuildPillar()
    {
        pillar.SetActive(true);
        switchComponent.currentDialogue = placeHolderDialogueTwo;
    }
}
