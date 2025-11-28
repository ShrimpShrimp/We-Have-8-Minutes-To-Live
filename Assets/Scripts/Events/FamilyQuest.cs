using System;
using UnityEngine;
using System.Collections;

public class FamilyQuest : MonoBehaviour
{
    

    [Header("Transforms and Dialogue Components")]
    public DialogueManager dialogueManager;
    public BranchManager branchManager;
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
    public DialogueAsset momDinnerReady;
    public DialogueAsset sisterFailsafe;
    public DialogueAsset sisterNotJoin;
    public DialogueAsset oneParentsDinner;
    public DialogueAsset parentsGettingSister;
    public DialogueAsset familyDinner;
    public DialogueAsset familyPleaseStay;
    public DialogueAsset sisterRickyMoment;
    public DialogueAsset endFamilyGood;
    public DialogueAsset endFamilyGood2;

    [Header("Sprites and Renderers")]
    public SpriteRenderer sisterSprite;
    public Sprite sisterCryingSprite;

    [Header("Teleport Locations")]
    public Vector3 kitchenDoor = new Vector3 (-0.909f, -0.898f, -6.707f);
    public Vector3 kitchenSpot = new Vector3 (-5.918f, -0.898f, -6.707f);
    public Vector3 bedDoor = new Vector3 (4.012f, -0.898f, -8.307f);
    public Vector3 bedSpot = new Vector3 (1.659f, -0.898f, -11.291f);
    public Vector3 diningDoor = new Vector3 (-4.23f, -0.898f, -9.49f);
    public Vector3 sisTable = new Vector3(-6.034f, -0.898f, -10.977f);
    public Vector3 momTable = new Vector3(-7.04f, -0.898f, -10.977f);
    public Vector3 dadTable = new Vector3(-8.342f, -0.898f, -12.397f);
    public Vector3 frontDoor = new Vector3(3.443f, -0.898f, -3.102f); //get this location
    public Vector3 nextToRicky = new Vector3(-0.06f, -0.898f, 6.86f); //also get this location
    public Vector3 nextToMom = new Vector3(-6.462f, -0.898f, -10.977f); //get this location

    [Header("Timings and location check")]
    public float setTableTime = 360f;
    public bool canCallDinner = true;
    public bool insideHouse = true;
    private bool missedDinner = false;
    private bool sisterUpset = false;

    [Header("Script References")]
    public PlayerMovement playerMovement;
    public DeathPlaneMove deathTimer;
    public PhoneManager phone;
    public PlayerInteraction interaction;
    public Footsteps footsteps;


    void Start()
    {
        EventManager.StartListening("2IntroParents", TwoIntroParents);
        EventManager.StartListening("upsetParents", UpsetParents);
        EventManager.StartListening("dadRepeatEarly", DadRepeatEarly);
        EventManager.StartListening("DadWaitEarly", DadWaitEarly);
        EventManager.StartListening("momDot", MomDot);
        EventManager.StartListening("SisterFurious", SisterFurious);
        fatherComponent.Interact();
        RunTimerMethod(setTableTime, MomDinnerReady);
        EventManager.StartListening("SetDinner", SetDinner);
        EventManager.StartListening("SisterFailsafe", SisterFailsafe);
        EventManager.StartListening("SisterToDinner", SisterToDinner);
        EventManager.StartListening("ParentsGettingSister", ParentsGettingSister);
        EventManager.StartListening("EndDinnerEarly", EndDinnerEarly);
        EventManager.StartListening("gamePauseTillEndFamily", GamePauseTillEndFamily);
        EventManager.StartListening("SisterRushToMom", SisterRushToMom);
        EventManager.StartListening("PauseToEnd", PauseToEnd);
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
        NPCMoveToSpot.MoveToPosition(this, motherTransform, kitchenDoor, 4f);
        NPCMoveToSpot.MoveToPosition(this, motherTransform, kitchenSpot, 4f);
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
        sisterUpset = true;
        sisterSprite.sprite = sisterCryingSprite;
        sisterComponent.currentDialogue = sisterCrying;
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, bedDoor, 6f);
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, bedSpot, 6f);
        //fade in
    }

    private void MomDinnerReady()
    {
        if (insideHouse)
        {
            if (!sisterUpset)
            {
                motherComponent.currentDialogue = sisterFailsafe;
                motherComponent.Interact();
            } else
            {
                motherComponent.currentDialogue = momDinnerReady;
                motherComponent.Interact();
            }
        } else
        {
            missedDinner = true;
            Debug.Log("You missed dinner. This isn't implemented yet.");
            //set up changes for if you missed dinner **IMPORTANT**
        }
    }

    private void SisterFailsafe()
    {
        sisterUpset = true;
        sisterSprite.sprite = sisterCryingSprite;
        sisterComponent.currentDialogue = sisterNotJoin;
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, bedDoor, 6f);
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, bedSpot, 6f);
        NPCMoveToSpot.MoveToPosition(this, motherTransform, diningDoor, 6f);
        NPCMoveToSpot.MoveToPosition(this, motherTransform, sisTable, 4f);
        //place plate at sis place **IMPLEMENT**
        NPCMoveToSpot.MoveToPosition(this, motherTransform, momTable, 4f);
        //place plate at mom place
        NPCMoveToSpot.MoveToPosition(this, motherTransform, dadTable, 4f);
        //place plate at dad place
        NPCMoveToSpot.MoveToPosition(this, motherTransform, momTable, 4f);
        motherComponent.currentDialogue = oneParentsDinner;
        NPCMoveToSpot.MoveToPosition(this, fatherTransform, kitchenDoor, 4f);
        NPCMoveToSpot.MoveToPosition(this, fatherTransform, diningDoor, 4f);
        NPCMoveToSpot.MoveToPosition(this, fatherTransform, momTable, 4f);
        NPCMoveToSpot.MoveToPosition(this, fatherTransform, dadTable, 4f);
        fatherComponent.currentDialogue = oneParentsDinner;
    }

    private void SetDinner()
    {
        sisterComponent.currentDialogue = sisterNotJoin;
        NPCMoveToSpot.MoveToPosition(this, motherTransform, diningDoor, 6f);
        NPCMoveToSpot.MoveToPosition(this, motherTransform, sisTable, 4f);
        //place plate at sis place **IMPLEMENT**
        NPCMoveToSpot.MoveToPosition(this, motherTransform, momTable, 4f);
        //place plate at mom place
        NPCMoveToSpot.MoveToPosition(this, motherTransform, dadTable, 4f);
        //place plate at dad place
        NPCMoveToSpot.MoveToPosition(this, motherTransform, momTable, 4f);
        motherComponent.currentDialogue = oneParentsDinner;
        NPCMoveToSpot.MoveToPosition(this, fatherTransform, kitchenDoor, 4f);
        NPCMoveToSpot.MoveToPosition(this, fatherTransform, diningDoor, 4f);
        NPCMoveToSpot.MoveToPosition(this, fatherTransform, momTable, 4f);
        NPCMoveToSpot.MoveToPosition(this, fatherTransform, dadTable, 4f);
        fatherComponent.currentDialogue = oneParentsDinner;
    }

    private void ParentsGettingSister()
    {
        motherComponent.currentDialogue = parentsGettingSister;
        fatherComponent.currentDialogue = parentsGettingSister;
    }

    private void SisterToDinner()
    {
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, bedDoor, 4f);
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, kitchenDoor, 4f);
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, diningDoor, 4f);
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, sisTable, 4f);
        sisterComponent.currentDialogue = familyDinner;
        motherComponent.currentDialogue = familyDinner;
        fatherComponent.currentDialogue = familyDinner;
    }

    private void EndDinnerEarly()
    {
        fatherComponent.currentDialogue = familyPleaseStay;
        motherComponent.currentDialogue = familyPleaseStay;
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, frontDoor, 6f);
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, nextToRicky, 6f);
        sisterComponent.currentDialogue = sisterRickyMoment;
    }

    private void GamePauseTillEndFamily()
    {
        StartCoroutine(GamePauseTillEndFamilyRoutine());
    }

    private IEnumerator GamePauseTillEndFamilyRoutine()
    {
        fatherComponent.currentDialogue = endFamilyGood;
        playerMovement.walkSpeed = 0;
        playerMovement.sprintSpeed = 0;
        phone.canUsePhone = false;
        interaction.canInteract = false;
        footsteps.shutUp = true;
        // Wait until the death timer hits exactly 30
        yield return new WaitUntil(() => deathTimer.remainingTime <= 30f);
        Debug.Log("Pause for family over");
        phone.canUsePhone = true;
        playerMovement.walkSpeed = 3;
        playerMovement.sprintSpeed = 7;
        interaction.canInteract = true;
        footsteps.shutUp = false;
        fatherComponent.Interact();
    }

    private void SisterRushToMom()
    {
        playerMovement.walkSpeed = 0;
        playerMovement.sprintSpeed = 0;
        phone.canUsePhone = false;
        interaction.canInteract = false;
        footsteps.shutUp = true;
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, nextToMom, 2f);
        fatherComponent.currentDialogue = endFamilyGood2;
        playerMovement.walkSpeed = 3;
        playerMovement.sprintSpeed = 7;
        phone.canUsePhone = true;
        interaction.canInteract = true;
        footsteps.shutUp = false;
        fatherComponent.Interact();
    }

    private void PauseToEnd()
    {
        playerMovement.walkSpeed = 0;
        playerMovement.sprintSpeed = 0;
        footsteps.shutUp = true;
        interaction.canInteract = false;
        phone.canUsePhone = false;
    }

    public void RunTimerMethod(float delay, Action method)
    {
        StartCoroutine(RunTimerCoroutine(delay, method));
    }

    private IEnumerator RunTimerCoroutine(float delay, Action method)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            while (dialogueManager.dialogueActive || branchManager.branchActive)
            {
                yield return null;
            }

            float buffer = 0.5f;
            float elapsed = 0f;
            bool aborted = false;

            while (elapsed < buffer)
            {
                if ((dialogueManager != null && dialogueManager.dialogueActive)
                    || (branchManager != null && branchManager.branchActive))
                {
                    aborted = true;
                    break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
            if (!aborted)
                break;

        }

        method?.Invoke();
    }
}
