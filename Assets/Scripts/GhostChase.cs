using UnityEngine;

public class GhostChase : GhostBehaviour
{
    private void OnDisable() {
        ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled) { // valid node, chase behaviour enabled
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue; // ram usage goes brrr

            foreach (Vector2 availableDirection in node.availableDirections) {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float distanceToTarget = (ghost.target.position - newPosition).sqrMagnitude;

                if (distanceToTarget < minDistance) {
                    direction = availableDirection;
                    minDistance = distanceToTarget;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }
}
