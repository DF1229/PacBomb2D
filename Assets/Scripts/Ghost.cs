using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(GhostHome))]
[RequireComponent(typeof(GhostScatter))]
[RequireComponent(typeof(GhostChase))]
[RequireComponent(typeof(GhostVulnerable))]
public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostVulnerable vulnerable { get; private set; }
    public GhostBehaviour initialBehaviour;

    public LayerMask bombLayer;
    public LayerMask pacmanLayer;

    public Transform target;
    public int points = 200;

    private void Awake() {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        vulnerable = GetComponent<GhostVulnerable>();
    }

    private void Start() {
        ResetState();
    }

    public void ResetState() {
        gameObject.SetActive(true);
        movement.ResetState();

        vulnerable.Disable();
        chase.Disable();
        scatter.Enable();
        
        if (home != initialBehaviour) {
            home.Disable();
        }

        if (initialBehaviour != null) {
            initialBehaviour.Enable();
        }
    }

    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (vulnerable.enabled && col.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            GameManager.Instance.GhostExploded(this);
        } else if (col.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            GameManager.Instance.PacmanEaten();
        }
    }
}