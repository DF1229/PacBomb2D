using UnityEngine;

public class Passage : MonoBehaviour
{
    public Transform connection;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 position = other.transform.position; // get position of object that collided with trigger
        position.x = connection.position.x; // set x to connector's x
        position.y = connection.position.y; // set y to connector's y

        other.transform.position = position; // set object's position to that of the connection
    }
}
