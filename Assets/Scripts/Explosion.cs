using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float lifeTime = 1f;

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
}