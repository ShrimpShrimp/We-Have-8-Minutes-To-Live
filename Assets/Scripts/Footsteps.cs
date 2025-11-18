using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public AudioSource audioSource;          // The AudioSource to play footstep sounds
    public AudioClip defaultWalkClip;        // General walking clip, swappable
    public AudioClip defaultSprintClip;      // General sprinting clip, swappable

    public float fadeSpeed = 3f;              // How fast volume fades in/out
    public float targetVolume = 1f;           // Max volume when walking/sprinting

    [Tooltip("Assign your player controller script here")]
    public PlayerMovement playerController; // Reference to your movement script to check canMove

    private AudioClip currentClip;            // Clip currently playing (walk or sprint)

    void Update()
    {
        if (playerController == null || audioSource == null)
            return;

        // If player cannot move, fade out and stop any sound
        if (!playerController.canMove)
        {
            FadeOutAndStop();
            return;
        }

        // Check if movement keys are pressed
        bool walkingNow = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                          Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
                          Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) ||
                          Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow);

        if (!walkingNow)
        {
            FadeOutAndStop();
            return;
        }

        // Determine which clip to use based on sprint key
        AudioClip desiredClip = Input.GetKey(KeyCode.LeftShift) ? defaultSprintClip : defaultWalkClip;

        // If clip changed or audio stopped, switch clip and play
        if (audioSource.clip != desiredClip || !audioSource.isPlaying)
        {
            audioSource.clip = desiredClip;
            audioSource.Play();
        }

        // Fade volume in
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, fadeSpeed * Time.deltaTime);
    }

    private void FadeOutAndStop()
    {
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, 0f, fadeSpeed * Time.deltaTime);
        if (audioSource.volume <= 0.01f && audioSource.isPlaying)
            audioSource.Stop();
    }
}
