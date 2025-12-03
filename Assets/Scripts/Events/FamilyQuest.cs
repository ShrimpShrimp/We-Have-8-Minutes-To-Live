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
    public DialogueAsset zeroDadEarly;
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
    public DialogueAsset missedDinnerDad;
    public DialogueAsset missedDinnerSister;
    public DialogueAsset missedDinnerMom;
    public DialogueAsset sisDot;
    public DialogueAsset sisNotCome;
    public DialogueAsset endFamilyBad;
    public DialogueAsset defeatedEnding;
    public DialogueAsset repeatDadEnding;
    public DialogueAsset winningEnding;
    public DialogueAsset theEnd;
    public DialogueAsset familyFreed;

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
    public Vector3 frontDoor = new Vector3(3.443f, -0.898f, -3.102f);
    public Vector3 nextToRicky = new Vector3(-0.06f, -0.898f, 6.86f); //get this location
    public Vector3 nextToMom = new Vector3(-6.462f, -0.898f, -10.977f);
    public Vector3 outFrontDoor = new Vector3(0f, -0.898f, 0f);
    public Vector3 momRoom = new Vector3(0f, -0.898f, 0f);

    [Header("Timings and location check")]
    public float setTableTime = 360f;
    public float tooLateTime = 450f;
    public bool canCallDinner = true;
    public bool insideHouse = true;
    private bool missedDinner = false;
    private bool sisterUpset = false;
    private bool familyUpset = false;

    [Header("Script References")]
    public PlayerMovement playerMovement;
    public DeathPlaneMove deathTimer;
    public PhoneManager phone;
    public PlayerInteraction interaction;
    public Footsteps footsteps;


    void Start()
    {
        EventManager.StartListening("2IntroParents", TwoIntroParents);
        EventManager.StartListening("1DadEarly", OneDadEarly);
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
        EventManager.StartListening("SisterNotCome", SisterNotCome);
        EventManager.StartListening("GamePauseTillEndParents", GamePauseTillEndParents);
        RunTimerMethod(tooLateTime, TooLateDinner);
        EventManager.StartListening("RepeatDadEnding", RepeatDadEnding);
    }

    private void TwoIntroParents()
    {
        motherComponent.currentDialogue = oneMomEarly;
        fatherComponent.currentDialogue = zeroDadEarly;
        sisterComponent.currentDialogue = oneSisterEarly;
        NPCMoveToSpot.MoveToPosition(this, motherTransform, kitchenDoor, 4f);
        NPCMoveToSpot.MoveToPosition(this, motherTransform, kitchenSpot, 4f);
    }

    private void OneDadEarly()
    {
        fatherComponent.currentDialogue = oneDadEarly;
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
            familyUpset = true;
            Debug.Log("You missed dinner.");
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
            NPCMoveToSpot.MoveToPosition(this, fatherTransform, kitchenDoor, 4f);
            NPCMoveToSpot.MoveToPosition(this, fatherTransform, diningDoor, 4f);
            NPCMoveToSpot.MoveToPosition(this, fatherTransform, momTable, 4f);
            NPCMoveToSpot.MoveToPosition(this, fatherTransform, dadTable, 4f);
            fatherComponent.currentDialogue = missedDinnerDad;
            motherComponent.currentDialogue = missedDinnerMom;
            sisterComponent.currentDialogue = missedDinnerSister;
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
        familyUpset = true;
        fatherComponent.currentDialogue = familyPleaseStay;
        motherComponent.currentDialogue = familyPleaseStay;
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, frontDoor, 6f);
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, nextToRicky, 6f);
        sisterComponent.currentDialogue = sisterRickyMoment;
    }

    private void GamePauseTillEndFamily()
    {
        fatherComponent.currentDialogue = familyFreed;
        motherComponent.currentDialogue = familyFreed;
        sisterComponent.currentDialogue = familyFreed;
        StartCoroutine(GamePauseTillEndFamilyRoutine());
    }

    private IEnumerator GamePauseTillEndFamilyRoutine()
    {
        // Wait until the death timer hits exactly 30
        yield return new WaitUntil(() => deathTimer.remainingTime <= 30f);
        fatherComponent.currentDialogue = endFamilyGood;
        Debug.Log("Pause for family over");
        if (insideHouse)
        {
            fatherComponent.Interact();
        } else
        {
            Debug.Log("You missed everything + family is not upset.");
            NPCMoveToSpot.MoveToPosition(this, sisterTransform, nextToRicky, 6f);
            NPCMoveToSpot.MoveToPosition(this, fatherTransform, outFrontDoor, 4f);
            fatherComponent.currentDialogue = winningEnding;
            NPCMoveToSpot.MoveToPosition(this, motherTransform, momRoom, 6f);
        }
    }

    private void SisterRushToMom()
    {
        NPCMoveToSpot.MoveToPosition(this, sisterTransform, nextToMom, 2f);
        fatherComponent.currentDialogue = endFamilyGood2;
        fatherComponent.Interact();
    }

    private void PauseToEnd()
    {
        fatherComponent.currentDialogue = theEnd;
        sisterComponent.currentDialogue = sisDot;
        motherComponent.currentDialogue = theEnd;
    }

    private void SisterNotCome()
    {
        sisterComponent.currentDialogue = sisDot;
        fatherComponent.currentDialogue = sisNotCome;
        motherComponent.currentDialogue = sisNotCome;
        familyUpset = false;
    }

    private void GamePauseTillEndParents()
    {
        fatherComponent.currentDialogue = familyFreed;
        motherComponent.currentDialogue = familyFreed;
        StartCoroutine(GamePauseTillParentsRoutine());
    }

    private IEnumerator GamePauseTillParentsRoutine()
    {
        // Wait until the death timer hits exactly 30
        yield return new WaitUntil(() => deathTimer.remainingTime <= 30f);
        Debug.Log("Pause for family over");
        fatherComponent.currentDialogue = endFamilyBad;
        motherComponent.currentDialogue = endFamilyBad;
        sisterComponent.currentDialogue = familyFreed;
        if (insideHouse)
        {
            fatherComponent.Interact();
        } else
        {
            Debug.Log("You completely missed everything + family is upset. Implement.");
            NPCMoveToSpot.MoveToPosition(this, sisterTransform, nextToRicky, 6f);
            NPCMoveToSpot.MoveToPosition(this, fatherTransform, outFrontDoor, 4f);
            fatherComponent.currentDialogue = defeatedEnding;
            NPCMoveToSpot.MoveToPosition(this, motherTransform, momRoom, 6f);
        }
    }

    private void TooLateDinner()
    {
        if (familyUpset)
        {
            Debug.Log("You completely missed everything + family is upset. Implement.");
            NPCMoveToSpot.MoveToPosition(this, sisterTransform, nextToRicky, 6f);
            NPCMoveToSpot.MoveToPosition(this, fatherTransform, outFrontDoor, 4f);
            fatherComponent.currentDialogue = defeatedEnding;
            NPCMoveToSpot.MoveToPosition(this, motherTransform, momRoom, 6f);
        } else if (!familyUpset && !insideHouse)
        {
            Debug.Log("You missed everything + family is not upset.");
            NPCMoveToSpot.MoveToPosition(this, sisterTransform, nextToRicky, 6f);
            NPCMoveToSpot.MoveToPosition(this, fatherTransform, outFrontDoor, 4f);
            fatherComponent.currentDialogue = winningEnding;
            NPCMoveToSpot.MoveToPosition(this, motherTransform, momRoom, 6f);
        }
    }

    private void RepeatDadEnding()
    {
        fatherComponent.currentDialogue = repeatDadEnding;
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
