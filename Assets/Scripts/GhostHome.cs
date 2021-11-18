using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehaviour
{
    public Transform inside;
    public Transform outside;
    public float transitionDuration = 0.5f;

    private void OnEnable() {
        StopAllCoroutines(); // fail-safe
    }

    private void OnDisable() {
        if (this.gameObject.activeSelf) {
            StartCoroutine(ExitTransition());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (this.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            ghost.movement.SetDirection(-ghost.movement.direction);
        }
    }

    private IEnumerator ExitTransition() {
        this.ghost.movement.SetDirection(Vector2.up, true); // force upward movement
        this.ghost.movement.rigidbody.isKinematic = true; // turn off collisions
        this.ghost.movement.enabled = false; // turn off movement script

        Vector3 position = transform.position;
        float elapsed = 0.0f;

        while (elapsed < transitionDuration) {
            Vector3 newPosition = Vector3.Lerp(position, inside.position, elapsed/transitionDuration);
            newPosition.z = position.z; // keep z (draworder) unchanged
            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null; // wait one frame after each loop (coroutine)
        }

        elapsed = 0.0f; // reset timer

        while (elapsed < transitionDuration) {
            Vector3 newPosition = Vector3.Lerp(inside.position, outside.position, elapsed/transitionDuration);
            newPosition.z = position.z; // keep z (draworder) unchanged
            ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null; // wait one frame after each loop (coroutine)
        }

        this.ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, 0.0f), true);
        this.ghost.movement.rigidbody.isKinematic = false; // turn on collisions
        this.ghost.movement.enabled = true; // turn on movement script
    }
}
