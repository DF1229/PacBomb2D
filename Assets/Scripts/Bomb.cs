using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float countdown = 2f;

    void Awake()
    {
        GameManager.Instance.bombs.Add(this);
    }

    void Update() {
        countdown -= Time.deltaTime;

        if (countdown <= 0f) {
            BombExploder.Instance.Explode(transform.position);

            if (GameManager.Instance.bombs.Contains(this))
                GameManager.Instance.bombs.Remove(this);
            else
                Debug.Log("Couldn't remove bomb instance from gamemanager!");
            
            Destroy(gameObject);
        }
    }
}
