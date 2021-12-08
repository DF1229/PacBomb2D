using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject bomb;
    public Movement movement { get; private set; }
    private bool _canAttack;
    public bool canAttack {
        get { return _canAttack; }   
        set {
            _canAttack = value;
            UIManager.Instance.UpdateAttackDisplay(value);
        }
    }

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {

        // Movement
        controls.Gameplay.MoveUp.performed += ctx => {
            movement.SetDirection(Vector2.up);
            Debug.Log("MoveUp.performed");
        };
        controls.Gameplay.MoveDown.performed += ctx => {
            movement.SetDirection(Vector2.down);
            Debug.Log("MoveDown.performed");
        };
        controls.Gameplay.MoveLeft.performed += ctx => {
            movement.SetDirection(Vector2.left);
            Debug.Log("MoveLeft.performed");
        };
        controls.Gameplay.MoveRight.performed += ctx => {
            movement.SetDirection(Vector2.right);
            Debug.Log("MoveRight.performed");
        };

        //controls.Gameplay.MoveUp.performed += ctx => movement.SetDirection(Vector2.up);
        //controls.Gameplay.MoveDown.performed += ctx => movement.SetDirection(Vector2.down);
        //controls.Gameplay.MoveLeft.performed += ctx => movement.SetDirection(Vector2.left);
        //controls.Gameplay.MoveRight.performed += ctx => movement.SetDirection(Vector2.right);

        // Attacking
        controls.Gameplay.Attack.performed += ctx => Attack();

        // Rotate pacman based on the current direction of travel
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x); 
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    private void Attack() {
        if (canAttack) {
            Vector3Int cellNum = tilemap.WorldToCell(transform.position);
            Vector3 cellPos = tilemap.GetCellCenterWorld(cellNum);
            cellPos.z = -2.5f; // -2.5f = distance from camera relative to world's 0 point, ensures correct draworder

            Instantiate(bomb, cellPos, Quaternion.identity);
            canAttack = false;

            foreach (Ghost ghost in GameManager.Instance.ghosts)
            {
                ghost.scatter.Enable(5f); // scatter for 5 seconds
            }
        }
    }

    public void ResetState() {
        movement.ResetState();
        gameObject.SetActive(true);
        canAttack = false;
    }
}
