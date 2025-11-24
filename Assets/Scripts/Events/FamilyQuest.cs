using UnityEngine;

public class FamilyQuest : MonoBehaviour
{
    public Transform motherTransform;
    public Transform fatherTransform;
    public Transform sisterTransform;
    public DialogueComponent motherComponent;
    public DialogueComponent fatherComponent;
    public DialogueComponent sisterComponent;

    public DialogueAsset oneDadEarly;
    public DialogueAsset oneMomEarly;
    public DialogueAsset oneSisterEarly;
    public DialogueAsset momAngry;
    public DialogueAsset dadAngry;
    public DialogueAsset dadRepeatEarly;
    public DialogueAsset dadWait;
    public DialogueAsset momDot;
    public DialogueAsset sisterCrying;

    void Start()
    {
        EventManager.StartListening("2IntroParents", TwoIntroParents);
        EventManager.StartListening("upsetParents", UpsetParents);
        EventManager.StartListening("dadRepeatEarly", DadRepeatEarly);
        EventManager.StartListening("DadWaitEarly", DadWaitEarly);
        EventManager.StartListening("momDot", MomDot);
        EventManager.StartListening("SisterFurious", SisterFurious);
        fatherComponent.Interact();
    }

    // Update is called once per frame
    void OnDestroy()
    {
        EventManager.StopListening("2IntroParents", TwoIntroParents);
        EventManager.StopListening("upsetParents", UpsetParents);
        EventManager.StopListening("dadRepeatEarly", DadRepeatEarly);
        EventManager.StopListening("DadWaitEarly", DadWaitEarly);
        EventManager.StopListening("momDot", MomDot);
        EventManager.StopListening("SisterFurious", SisterFurious);
    }

    private void TwoIntroParents()
    {
        //fade out
        motherComponent.currentDialogue = oneMomEarly;
        fatherComponent.currentDialogue = oneDadEarly;
        sisterComponent.currentDialogue = oneSisterEarly;
        //teleport mom to kitchen
        //fade back in
    }

    private void UpsetParents() 
    {
        motherComponent.currentDialogue = momAngry;
        motherComponent.currentDialogue = dadAngry;
    }

    private void DadRepeatEarly() 
    {
        fatherComponent.currentDialogue = dadRepeatEarly;
    }

    private void DadWaitEarly()
    {
        fatherComponent.currentDialogue = dadWait;
    }

    private void MomDot() 
    {
        motherComponent.currentDialogue = momDot;
    }

    private void SisterFurious() 
    {
        //fade out
        //change sister sprite to crying
        sisterComponent.currentDialogue = sisterCrying;
        //fade in
    }
}
