using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class GuitarScript : Interactable
{

    [Header("Scripts")]
    public PlayerMovement playerMovement;
    public DialogueManager dialogueManager;
    public FriendsQuest friendsQuest;
    public Footsteps footsteps;
    public PlayerInteraction interaction;
    public PhoneManager phoneManager;

    [Header("GameObjects/Audio")]
    public AudioSource eight;
    public AudioSource briefWalk;
    public AudioSource rage;
    public AudioSource footSounds;
    public GameObject guitar;
    public GameObject guitarUI;
    public GameObject bass;
    public GameObject drums;
    public GameObject keyboard;

    [Header("SpriteManagers")]
    public SpriteRenderer benRenderer;
    public SpriteRenderer tomRenderer;
    public SpriteRenderer eveRenderer;
    public SpriteIdler benIdler;
    public SpriteIdler tomIdler;
    public SpriteIdler eveIdler;
    public Sprite benPlay;
    public Sprite evePlay;
    public Sprite tomPlay;

    [Header("Transforms")]
    public Transform benTransform;
    public Transform tomTransform;
    public Transform eveTransform;
    public Vector3 keysPos = new Vector3(0f, 0f, 0f);
    public Vector3 bassPos = new Vector3 (0f, 0f, 0f);
    public Vector3 drumPos = new Vector3 (-0f, 0f, 0f);

    public override void Interact()
    {
        StartCoroutine(InteractRoutine());
    }

    private IEnumerator InteractRoutine()
    {
        phoneManager.canUsePhone = false;
        interaction.canInteract = false;
        guitarUI.SetActive(true);
        playerMovement.walkSpeed = 0f;
        playerMovement.sprintSpeed = 0f;
        footsteps.targetVolume = 0f;
        footSounds.volume = 0f;

        NPCMoveToSpot.MoveToPosition(this, eveTransform, keysPos, 5f);
        NPCMoveToSpot.MoveToPosition(this, tomTransform, drumPos, 5f);
        NPCMoveToSpot.MoveToPosition(this, benTransform, bassPos, 5f);

        //Pause for 1 second
        yield return new WaitForSeconds(1f);

        drums.SetActive(false);
        keyboard.SetActive(false);
        bass.SetActive(false);

        benIdler.idleSprite = benPlay;
        tomIdler.idleSprite = tomPlay;
        eveIdler.idleSprite = evePlay;

        benRenderer.sprite = benPlay;
        tomRenderer.sprite = tomPlay;
        eveRenderer.sprite = evePlay;

        eight.volume = 0f;
        briefWalk.volume = 0f;
        rage.Play();
        guitar.SetActive(false);
    }
}
