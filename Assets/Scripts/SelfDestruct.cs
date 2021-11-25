using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float timer = 1.5f;

    public void Start()
    {
        Invoke(nameof(Die), timer);
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
