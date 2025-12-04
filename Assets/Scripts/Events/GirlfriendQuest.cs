using System;
using UnityEngine;
using System.Collections;

public class GirlfriendQuest : MonoBehaviour
{
    [Header("Transforms and Dialogue Components")]
    public DialogueComponent stephComponent;
    public DialogueComponent playerComponent;

    [Header("Dialogue Assets")]
    public DialogueAsset endCall;
    public DialogueAsset noResponse;
    public DialogueAsset insideDontCall;
    public DialogueAsset stephCallIntro;
    public DialogueAsset loveLoopEntryOne;
    public DialogueAsset callBackOne;
    public DialogueAsset lateRecall;

    [Header("Sprites and Renderers")]
    public GameObject stephPhone;
    public SpriteRenderer stephSpriteRenderer;

    [Header("Timings and location check")]
    public float fourMins = 240f;
    public float deathTime = 420f;

    [Header("Script References")]
    public PhoneManager phone;
    public DialogueManager dialogueManager;
    public BranchManager branchManager;
    public FamilyQuest family;

    private bool stephAlive = true;
    private bool stephHappy = false;
    private bool setToLate = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stephComponent.currentDialogue = stephCallIntro;

        EventManager.StartListening("EndCall", EndCallSteph);
        RunTimerMethod(deathTime, RecieverCutoff);
        RunTimerMethod(fourMins, LateRecallTimer);
        EventManager.StartListening("HungUpOne", HungUpOne);
        EventManager.StartListening("LeaveCallGood", LeaveCallGood);
        EventManager.StartListening("LeaveLoveLoop", LeaveLoveLoop);
        EventManager.StartListening("LateRecall", LateRecall);
    }


    private void Update()
    {
        if (!stephPhone.activeSelf && !stephHappy && setToLate)
        {
            LateRecall();
            setToLate = false;
        }
    }

    public void LateRecallTimer()
    {
        
            setToLate = true;
    }


    public void RecieverCutoff()
    {
        Debug.Log("Cutt off phone call");
        if (stephPhone.activeSelf && dialogueManager.dialogueActive)
        {
            Debug.Log("Successfully force end of call");
            dialogueManager.ForceCloseDialogueSafe();
            EndCallSteph();
            StartCoroutine(DeadCallRoutine());
        }
        else if (stephPhone.activeSelf && branchManager.branchActive)
        {
            branchManager.ForceCloseBranchSafe();
            EndCallSteph();
            StartCoroutine(DeadCallRoutine());
        }
            stephAlive = false;
    }

    private IEnumerator DeadCallRoutine()
    {
        yield return null;  // wait one frame to ensure force close finishes

        playerComponent.currentDialogue = endCall;
        playerComponent.Interact();
    }

    public void CallSteph()
    {
        if (!family.insideHouse && stephAlive)
        {
            stephPhone.SetActive(true);
            stephComponent.Interact();
        } else if (family.insideHouse) 
        {
            playerComponent.currentDialogue = insideDontCall;
            playerComponent.Interact();
        } else
        {
            playerComponent.currentDialogue = noResponse;
            playerComponent.Interact();
        }
        
    }

    public void EndCallSteph()
    {
        stephPhone.SetActive(false);
    }

    public void HungUpOne()
    {
        stephComponent.currentDialogue = callBackOne;
        stephPhone.SetActive(false);
    }


    public void LeaveCallGood()
    {
        stephHappy = true;
        stephPhone.SetActive(false);
        stephComponent.currentDialogue = loveLoopEntryOne;
    }

    public void LeaveLoveLoop()
    {
        stephHappy = true;
        stephPhone.SetActive(false);
        stephComponent.currentDialogue = loveLoopEntryOne;
    }

    public void LateRecall()
    {
        stephPhone.SetActive(false);
        stephComponent.currentDialogue = lateRecall;
    }

    public void RunTimerMethod(float delay, Action method)
    {
        StartCoroutine(RunTimerCoroutine(delay, method));
    }

    private IEnumerator RunTimerCoroutine(float delay, Action method)
    {
        Debug.Log($"Waiting for {delay} seconds before invoking {method.Method.Name}");
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }
}
