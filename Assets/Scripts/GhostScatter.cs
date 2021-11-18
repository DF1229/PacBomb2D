using UnityEngine;

public class GhostScatter : GhostBehaviour
{
    private void OnDisable() {
        ghost.chase.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled) { // valid node, behaviour enabled
            int index = Random.Range(0, node.availableDirections.Count); // pick random direction

            if (node.availableDirections[index] == -ghost.movement.direction) { // valid direction, check if it's a reverse of the current direction
                index++; // how to cause overflow issues 101

                if (index >= node.availableDirections.Count) { // how to prevent overflow issues 101
                    index = 0;
                }
            }

            ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }
}
