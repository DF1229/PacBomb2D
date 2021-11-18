using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int points = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == LayerMask.NameToLayer("Pacman"))
        {
            GameManager.Instance.FruitEaten(this);
        }
    }
}