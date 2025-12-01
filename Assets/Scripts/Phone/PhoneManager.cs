using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PhoneManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject phonePanel;
    public TextMeshProUGUI contactHeaderText;
    public Button callButton;
    public TextMeshProUGUI callButtonText;

    public Transform contactsContentParent;  // ContactsPanel/Viewport/Content
    public GameObject contactButtonPrefab;

    public Transform messagesContentParent;  // MessagesPanel/Viewport/Content
    public GameObject messageBubblePrefab;

    [Header("Contacts List")]
    public List<ContactAsset> contacts = new List<ContactAsset>();

    private ContactAsset currentContact;

    private List<GameObject> contactButtons = new List<GameObject>();
    private List<GameObject> messageBubbles = new List<GameObject>();

    [Header("Dialogue/Branch/Movement")]
    public DialogueManager dialogueManager;
    public BranchManager branchManager;
    public PlayerMovement playerMovement;
    public DialogueComponent playerComponent;
    public DialogueAsset brokenDialogue;

    [Header("Phone Bool")]
    public bool canUsePhone = true;

    void Start()
    {
        PopulateContacts();
        phonePanel.SetActive(false);

        callButton.onClick.AddListener(OnCallButtonClicked);

        if (contacts.Count > 0) contacts[0].callAction = CallGirlfriend;
        if (contacts.Count > 1) contacts[1].callAction = CallMom;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PhoneOnOff();
        }
    }

    private void PhoneOnOff()
    {
        if (!dialogueManager.dialogueActive && !branchManager.branchActive && canUsePhone)
        {
            phonePanel.SetActive(!phonePanel.activeSelf);
        }
        if (phonePanel.activeSelf)
        {
            playerMovement.canLook = false;
            playerMovement.canMove = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            playerMovement.canLook = true;
            playerMovement.canMove = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void PopulateContacts()
    {
        foreach (var contact in contacts)
        {
            GameObject btnObj = Instantiate(contactButtonPrefab, contactsContentParent);
            TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = contact.contactName;

            Button btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(() => OnContactSelected(contact));

            contactButtons.Add(btnObj);
        }

        if (contacts.Count > 0)
            OnContactSelected(contacts[0]);
    }

    void OnContactSelected(ContactAsset contact)
    {
        currentContact = contact;
        contactHeaderText.text = contact.contactName;

        ClearMessages();

        foreach (var msg in contact.textsAsset.messages)
        {
            AddMessageBubble(msg);
        }
    }

    void AddMessageBubble(string message)
    {
        GameObject bubble = Instantiate(messageBubblePrefab, messagesContentParent);
        TextMeshProUGUI bubbleText = bubble.GetComponentInChildren<TextMeshProUGUI>();
        bubbleText.text = message;

        // Optional: you can set color or styling here based on sender or other data

        messageBubbles.Add(bubble);
    }

    void ClearMessages()
    {
        foreach (var msgBubble in messageBubbles)
        {
            Destroy(msgBubble);
        }
        messageBubbles.Clear();
    }

    void OnCallButtonClicked()
    {
        if (currentContact != null)
        {
            currentContact.Call();
        }
    }

    // Example call behaviors (to be assigned to contacts)
    public void CallGirlfriend()
    {
        Debug.Log("Calling girlfriend");
        playerComponent.currentDialogue = brokenDialogue;
        PhoneOnOff();
        playerComponent.Interact();
        // You can open a dialogue or trigger whatever here
    }

    public void CallMom()
    {
        playerComponent.currentDialogue = brokenDialogue;
        PhoneOnOff();
        playerComponent.Interact();
        Debug.Log("Calling mom");
    }
}
