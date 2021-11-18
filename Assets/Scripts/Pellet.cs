using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int points = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Eat();
        }
    }

    protected virtual void Eat() // protected = subclasses can access, virtual = subclasses can override
    {
        GameManager.Instance.PelletEaten(this);
    }
}
