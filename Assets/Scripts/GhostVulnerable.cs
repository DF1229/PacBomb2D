using UnityEngine;

public class GhostVulnerable : GhostBehaviour
{
    [SerializeField] // somehow critical to prevent null reference exceptioms
    public float vulnerableSpeedMod = 0.75f;
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;

    public bool dead { get; private set; }

    public override void Enable(float duration)
    {
        base.Enable(duration);

        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        Invoke(nameof(Flash), duration / 2.5f);
    }

    public override void Disable()
    {
        base.Disable();

        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    public void Killed() 
    {
        dead = true;
        ghost.SetPosition(ghost.home.inside.position);
        ghost.home.Enable(8.0f);

        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    // Flash the ghost to indicate the vulnerability is about to wear off
    private void Flash()
    {
        if (!dead)
        {
            blue.enabled = false;
            white.enabled = true;
            white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    private void OnEnable() // called by unity
    {
        blue.GetComponent<AnimatedSprite>().Restart();
        ghost.movement.speedMultiplier = vulnerableSpeedMod;
        dead = false;
    }

    private void OnDisable() // called by unity
    {
        ghost.movement.speedMultiplier = 1.0f;
        dead = false;
    }

}