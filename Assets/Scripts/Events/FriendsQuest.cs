using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class FriendsQuest : MonoBehaviour
{
    [Header("Scripts")]
    public DialogueManager dialogueManager;
    public BranchManager branchManager;
    public PlayerMovement playerMovement;
    public DeathPlaneMove deathTimer;
    public PhoneManager phone;
    public PlayerInteraction interaction;
    


    [Header("Transforms and Dialogue Components")]
    public DialogueComponent tomComponent;
    public DialogueComponent benComponent;
    public DialogueComponent eveComponent;
    public DialogueComponent playerComponent;
    public Collider guitarCollider;

    [Header("Dialogue Assets")]
    public DialogueAsset setupPutOff;
    public DialogueAsset tomGetBen;
    public DialogueAsset benSetUp;
    public DialogueAsset benSetUpRepeat;
    public DialogueAsset youGetEve;
    public DialogueAsset youGetEveTwo;
    public DialogueAsset readyJam;
    public DialogueAsset eveNoPoint;
    public DialogueAsset eveNoPointRepeat;
    public DialogueAsset benReady;
    public DialogueAsset eveReady;
    public DialogueAsset tomReady;
    public DialogueAsset tomNoPoint;
    public DialogueAsset tomNoPointRepeat;
    public DialogueAsset tomNoPointTwo;
    public DialogueAsset benNoPoint;
    public DialogueAsset benNoPointRepeat;
    public DialogueAsset eveTheEnd;
    public DialogueAsset tomTheEnd;
    public DialogueAsset benTheEnd;
    public DialogueAsset benDefaultRepeat;
    public DialogueAsset eveDefaultRepeat;

    private bool eveSpoken = false;
    private bool tomSpoken = false;
    private bool sixMins = false;
    private bool gotEve = false;
    private bool isJamming = false;

    [Header("Teleport Locations/Transforms")]
    public Transform eveTransform;
    public Transform tomTransform;
    public Transform benTransform;
    public Vector3 tomBenHouse = new Vector3(0f, 0f, 0f);
    public Vector3 eveBenHouse = new Vector3(0f, 0f, 0f);

    [Header("Timings")]
    public float latePartyTime = 240f;
    public float lateNotStartTime = 360f;
    public float lastDialogueTime = 420f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.StartListening("PutOffQuest", PutOffQuest);
        EventManager.StartListening("GoGetBand", GoGetBand);
        EventManager.StartListening("BenSetUpRepeat", BenSetUpRepeat);
        EventManager.StartListening("GotEve", GotEve);
        EventManager.StartListening("ReadyGuitar", ReadyGuitar);
        RunTimerMethod(latePartyTime, LateToTheParty);
        RunTimerMethod(lateNotStartTime, LateNotStarted);
        RunTimerMethod(lastDialogueTime, LastDialogue);
        EventManager.StartListening("EveNoPointRepeat", EveNoPointRepeat);
        EventManager.StartListening("TomNoPointRepeat", TomNoPointRepeat);
        EventManager.StartListening("BenNoPointRepeat", BenNoPointRepeat);
        EventManager.StartListening("BenDefaultRepeat", BenDefaultRepeat);
        EventManager.StartListening("EveSpoken", EveSpoken);
    }

    private void PutOffQuest()
    {
        NPCMoveToSpot.MoveToPosition(this, tomTransform, tomBenHouse, 8f);
        benComponent.currentDialogue = setupPutOff;
        tomComponent.currentDialogue = setupPutOff;
        tomSpoken = true;
    }

    private void GoGetBand()
    {
        NPCMoveToSpot.MoveToPosition(this, tomTransform, tomBenHouse, 8f);
        tomComponent.currentDialogue = tomGetBen;
        benComponent.currentDialogue = benSetUp;
        if (!eveSpoken)
        {
            eveComponent.currentDialogue = youGetEve;
        } else
        {
            eveComponent.currentDialogue = youGetEveTwo;
        }
        tomSpoken = true;
    }

    private void BenSetUpRepeat()
    {
        benComponent.currentDialogue = benSetUpRepeat;
    }

    private void GotEve()
    {
        gotEve = true;
        NPCMoveToSpot.MoveToPosition(this, eveTransform, eveBenHouse, 8f);
        if (!sixMins)
        {
            tomComponent.currentDialogue = readyJam;
            benComponent.currentDialogue = readyJam;
            eveComponent.currentDialogue = readyJam;
        } else
        {
            eveComponent.currentDialogue = eveNoPoint;
        }
    }

    private void ReadyGuitar()
    {
        benComponent.currentDialogue = benReady;
        eveComponent.currentDialogue = eveReady;
        tomComponent.currentDialogue = tomReady;
        isJamming = true;
        guitarCollider.enabled = true;
    }

    private void LateToTheParty()
    {
        if (!tomSpoken)
        {
            NPCMoveToSpot.MoveToPosition(this, tomTransform, tomBenHouse, 8f);
            tomComponent.currentDialogue = tomNoPoint;
            benComponent.currentDialogue = benNoPoint;
        }
    }

    private void LateNotStarted()
    {
        if (!gotEve && tomSpoken)
        {
            tomComponent.currentDialogue = tomNoPointTwo;
            benComponent.currentDialogue = benNoPoint;
        }
    }

    private void LastDialogue()
    {
        if (!isJamming)
        {
            tomComponent.currentDialogue = tomTheEnd;
            eveComponent.currentDialogue = eveTheEnd;
            benComponent.currentDialogue = benTheEnd;
        }
        
    }

    private void EveNoPointRepeat()
    {
        eveComponent.currentDialogue = eveNoPointRepeat;
    }

    private void TomNoPointRepeat()
    {
        tomComponent.currentDialogue = tomNoPointRepeat;
    }

    private void BenNoPointRepeat()
    {
        benComponent.currentDialogue = benNoPointRepeat;
    }

    private void BenDefaultRepeat()
    {
        benComponent.currentDialogue = benDefaultRepeat;
    }

    private void EveSpoken()
    {
        eveComponent.currentDialogue = eveDefaultRepeat;
        eveSpoken = true;
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
