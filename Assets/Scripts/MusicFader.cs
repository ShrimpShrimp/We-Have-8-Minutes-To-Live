using UnityEngine;
using System.Collections;

public class MusicFader : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource trackA;  // Default area music
    public AudioSource trackB;  // Music inside the trigger

    [Header("Settings")]
    public float fadeDuration = 2f;

    private Coroutine currentFade;

    private void Start()
    {
        if (trackA != null && !trackA.isPlaying) trackA.Play();
        if (trackB != null && !trackB.isPlaying) trackB.Play();

        // Default volumes
        trackA.volume = 1f;
        trackB.volume = 0f;

        // Check if the player starts inside the trigger
        Collider[] overlaps = Physics.OverlapBox(transform.position, transform.localScale / 2f);
        foreach (Collider c in overlaps)
        {
            if (c.CompareTag("Player"))
            {
                // Already inside → start with trackB instead
                trackA.volume = 0f;
                trackB.volume = 1f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Crossfade A → B
            StartCrossfade(trackA, trackB);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Crossfade B → A
            StartCrossfade(trackB, trackA);
        }
    }

    private void StartCrossfade(AudioSource from, AudioSource to)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        currentFade = StartCoroutine(CrossfadeRoutine(from, to));
    }

    private IEnumerator CrossfadeRoutine(AudioSource from, AudioSource to)
    {
        float timer = 0f;

        float startFrom = from.volume;
        float startTo = to.volume;

        // Make sure both are playing, but not stopped
        if (!from.isPlaying) from.Play();
        if (!to.isPlaying) to.Play();

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            from.volume = Mathf.Lerp(startFrom, 0f, t);
            to.volume = Mathf.Lerp(startTo, 1f, t);

            yield return null;
        }

        from.volume = 0f;
        to.volume = 1f;
        currentFade = null;
    }
}
