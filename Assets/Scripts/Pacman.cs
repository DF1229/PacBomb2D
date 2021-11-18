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

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        // check movement axis
        if (Input.GetAxis("Vertical") > 0.0f) { // up
            movement.SetDirection(Vector2.up);
        } else if (Input.GetAxis("Vertical") < 0.0f) { // down
            movement.SetDirection(Vector2.down);
        } else if (Input.GetAxis("Horizontal") < 0.0f) { // left
            movement.SetDirection(Vector2.left);
        } else if (Input.GetAxis("Horizontal") > 0.0f) { // right
            movement.SetDirection(Vector2.right);
        }

        // check attack axis
        if (Input.GetAxis("Fire1") > 0.0f || Input.GetAxis("Fire2") > 0.0f) { // as determined by unity's input manager
            Attack();
        }

        // let there be movement!
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x); 
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    private void Attack() {
        if (canAttack) {
            Vector3Int cellNum = tilemap.WorldToCell(transform.position); // cell coords are in integers, using Vector3 just eats up more memory
            Vector3 cellPos = tilemap.GetCellCenterWorld(cellNum); // get location of tilemap cell's exact center
            cellPos.z = -2.5f; // display bomb above wall tiles

            Instantiate(bomb, cellPos, Quaternion.identity); // spawn bomb in cell
            canAttack = false;
        }
    }

    public void ResetState() {
        movement.ResetState();
        gameObject.SetActive(true);
        canAttack = false;
    }
}
