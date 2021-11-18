using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 14.0f;

    protected override void Eat()
    {
        GameManager.Instance.PowerPelletEaten(this);
    }
}
