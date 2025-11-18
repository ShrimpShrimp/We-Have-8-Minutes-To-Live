using UnityEngine;

public class SpriteLooper : MonoBehaviour
{
    [Header("Sprites to Loop")]
    public SpriteRenderer targetSprite;
    public Sprite[] sprites;
    public float switchInterval = 0.5f;

    private int currentIndex = 0;
    private float timer = 0f;


    void Update()
    {
        if (sprites == null || sprites.Length == 0 || targetSprite == null)
            return;


        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            currentIndex = (currentIndex + 1) % sprites.Length;
            targetSprite.sprite = sprites[currentIndex];
            timer = 0f;
        }
    }
}