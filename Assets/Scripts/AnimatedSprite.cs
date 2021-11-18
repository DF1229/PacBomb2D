using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public int animationFrame { get; private set; }
    public Sprite[] sprites;
    public float animationTime = 0.25f;
    public bool loop = true;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // #GotRekt
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance()
    {
        if(spriteRenderer == null || !spriteRenderer.enabled)
        {
            return;
        }

        animationFrame++;
        if (animationFrame >= sprites.Length && loop)
        {
            animationFrame = 0;
        }

        if (animationFrame >= 0 && animationFrame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[animationFrame];
        }

    }

    public void Restart()
    {
        animationFrame = -1;
        Advance();
    }

}