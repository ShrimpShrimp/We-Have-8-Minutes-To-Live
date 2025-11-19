using UnityEngine;

public class PlaceholderQuest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.StartListening("PLACEHOLDER_THREE", PlaceHolderThree);
        EventManager.StartListening("PLACEHOLDER_TWO", PlaceHolderTwo);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        EventManager.StopListening("PLACEHOLDER_THREE", PlaceHolderThree);
        EventManager.StopListening("PLACEHOLDER_TWO", PlaceHolderTwo);
    }

    private void PlaceHolderThree()
    {
        Debug.Log("Called placeholder three.");
    }

    private void PlaceHolderTwo()
    {
        Debug.Log("Called placeholder two.");
    }
}
