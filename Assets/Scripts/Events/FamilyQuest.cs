using UnityEngine;

public class FamilyQuest : MonoBehaviour
{
    [Header ("Transforms and Dialogue Components")]
    public Transform motherTransform;
    public Transform fatherTransform;
    public Transform sisterTransform;
    public DialogueComponent motherComponent;
    public DialogueComponent fatherComponent;
    public DialogueComponent sisterComponent;

    [Header("Dialogue Assets")]
    public DialogueAsset oneDadEarly;
    public DialogueAsset oneMomEarly;
    public DialogueAsset oneSisterEarly;
    public DialogueAsset momAngry;
    public DialogueAsset dadAngry;
    public DialogueAsset dadRepeatEarly;
    public DialogueAsset dadWait;
    public DialogueAsset momDot;
    public DialogueAsset sisterCrying;

    [Header("Sprites and Renderers")]
    public SpriteRenderer sisterSprite;
    public Sprite sisterCryingSprite;

    [Header("Teleport Locations")]
    public Vector3 momKitchenDoor;
    public Vector3 momKitchenSpot;
    public Vector3 sisterBedDoor;
    public Vector3 sisterBedSpot;

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
        motherComponent.currentDialogue = oneMomEarly;
        fatherComponent.currentDialogue = oneDadEarly;
        sisterComponent.currentDialogue = oneSisterEarly;
        NPCMoveToSpot.MoveToPosition(this, motherTransform, momKitchenDoor, 4f);
        NPCMoveToSpot.MoveToPosition(this, motherTransform, momKitchenSpot, 4f);
        //move mom to kitchen
    }

    private void UpsetParents() 
    {
        motherComponent.currentDialogue = momAngry;
        fatherComponent.currentDialogue = dadAngry;
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
        sisterSprite.sprite = sisterCryingSprite;
        sisterComponent.currentDialogue = sisterCrying;
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, sisterBedDoor, 6f);
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, sisterBedSpot, 6f);
        //fade in
    }
}
