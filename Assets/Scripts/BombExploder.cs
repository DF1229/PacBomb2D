using UnityEngine.Tilemaps;
using UnityEngine;

public class BombExploder : MonoBehaviour
{
    public int range = 5;
    public Tilemap tilemap; // bombable tilemap
    public RuleTile bombableRuleTile; // special ruletile to indicate where bombs can be placed
    public GameObject explosionPrefab;
    private static BombExploder _instance;
    public static BombExploder Instance {
        get {
            if (_instance == null) {
                Debug.LogError("BombExploder _instance is null!");
            }

            return _instance;
        }
    }
    // Singleton pattern: private static + public static with custom get/set ensures there will only ever be a single instance of this class.

    void Awake() {
        _instance = this;
    }

    public void Explode(Vector2 worldPos) {
        Vector3Int originCell = tilemap.WorldToCell(worldPos);

        Vector3Int[] up = new Vector3Int[range];
        Vector3Int[] down = new Vector3Int[range];
        Vector3Int[] left = new Vector3Int[range];
        Vector3Int[] right = new Vector3Int[range];

        for (int i = 1; i < range; i++)
        {
            up[i] = new Vector3Int(0, i, 0);
            down[i] = new Vector3Int(0, -i, 0);
            left[i] = new Vector3Int(-i, 0, 0);
            right[i] = new Vector3Int(i, 0, 0);
        }

        //TODO: 2d array?
        if(CheckCell(originCell))
        {
            ExplodeCell(originCell);
            foreach (Vector3Int cell in up)
            {
                if (CheckCell(originCell + cell))
                {
                    ExplodeCell(originCell + cell);
                } else
                {
                    break;
                }
            }
            foreach (Vector3Int cell in down)
            {
                if (CheckCell(originCell + cell))
                {
                    ExplodeCell(originCell + cell);
                } else
                {
                    break;
                }
            }
            foreach (Vector3Int cell in left)
            {
                if (CheckCell(originCell + cell))
                {
                    ExplodeCell(originCell + cell);
                } else
                {
                    break;
                }
            }
            foreach (Vector3Int cell in right)
            {
                if (CheckCell(originCell + cell))
                {
                    ExplodeCell(originCell + cell);
                } else
                {
                    break;
                }
            }
        }
    }

    private bool CheckCell(Vector3Int cell)
    {
        Tile tile = tilemap.GetTile<Tile>(cell); // attempt to find tile at bomb's position (should be null!)

        if (tile == null) // should evaluate to true
        {
            RuleTile ruleTile = tilemap.GetTile<RuleTile>(cell); // attempt to find a ruletile at the bomb's position (should exist)
            if (ruleTile != bombableRuleTile) // should evaluate to false
            {
                return false;
            }
        }

        return true;
    }

    private void ExplodeCell(Vector3Int cell) {
        Vector3 pos = tilemap.GetCellCenterWorld(cell);
        pos.z = -2.5f; // reassign draworder
        Instantiate(explosionPrefab, pos, Quaternion.identity); // instansiate explosion animation prefab
    }
}
