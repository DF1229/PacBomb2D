using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Public - Game elements
    public GameObject fakePacman; // death animation prefab
    public GameObject fruitPrefab;
    public GameObject gamePaused;
    public GameObject gameOver;
    public Transform pellets;
    public Pacman pacman;
    public List<Bomb> bombs = new List<Bomb>();
    public Sprite[] fruitSprites;
    public Ghost[] ghosts;

    // Private - Game elements
    private int remainingPellets;
    private int totalPellets;
    private bool fruitSpawned;
    private bool paused;
    private GameObject fruit;

    // Public - Stats set from editor
    public int explosionBonus = 20;
    public int ghostMultiplier { get; private set; }

    // Score singleton
    private static int _score { get; set; }
    public static int score
    {
        get { return _score; }
        set {
            _score = value;
            UIManager.Instance.UpdateScore(_score);
        }
    }

    // Lives singleton
    private int _lives;
    public int lives
    {
        get { return _lives; }
        set {
            _lives = value;
            UIManager.Instance.UpdateLives(value);
        }
    }

    // Gamemanager singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get {
            if (_instance == null)
            {
                Debug.LogError("GameManager _instance is null!");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        totalPellets = pellets.childCount;
        NewGame();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        ResetState();
        ResetPellets();
    }

    private void ResetState()
    {
        ResetGhostMultiplier();

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();
        }

        ClearBombs();
        pacman.ResetState();
    }

    private void SetScore(int newScore)
    {
        score = newScore;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void GhostExploded(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier + explosionBonus;
        SetScore(_score + points);
        ghostMultiplier++;

        ghost.vulnerable.Killed();
    }

    public void PacmanEaten()
    {
        ClearBombs();
        DisableGhosts();

        SpawnReplacementPacman();
    }

    

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(_score + pellet.points);
        remainingPellets -= 1;

        if (remainingPellets <= totalPellets / 2)
        {
            if (!fruitSpawned)
            {
                SpawnFruit();
            }
        } else if (!HasRemainingPellets())
        {
            Debug.Log("#rekt");
            // TODO: Victory!
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].vulnerable.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        pacman.canAttack = true;


        CancelInvoke();
        Invoke(nameof(DisablePacmanAttackState), pellet.duration);
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    public void FruitEaten(Fruit fruit)
    {
        fruit.gameObject.SetActive(false);
        SetScore(_score + fruit.points);

        ResetPellets();
        fruitSpawned = false;

        foreach (Ghost ghost in ghosts)
        {
            ghost.movement.speed = Mathf.MoveTowards(ghost.movement.speed, pacman.movement.speed, 0.3f);
        }
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void SpawnFruit()
    {
        int rand = Random.Range(0, fruitSprites.Length);
        Sprite sprite = fruitSprites[rand];

        fruit = Instantiate(fruitPrefab);
        SpriteRenderer fruitRenderer = fruit.GetComponent<SpriteRenderer>();
        fruitRenderer.sprite = sprite;

        fruitSpawned = true;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

    private void ResetPellets()
    {
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        remainingPellets = totalPellets;
    }

    private void DisablePacmanAttackState()
    {
        pacman.canAttack = false;
    }

    private void ClearBombs()
    {
        foreach (Bomb bomb in bombs)
        {
            if (bomb != null)
                Destroy(bomb.gameObject);
        }
    }

    private void DisableGhosts()
    {
        foreach (Ghost ghost in this.ghosts)
        {
            ghost.gameObject.SetActive(false);
        }
    }

    private void SpawnReplacementPacman()
    {
        Vector3 spawnPos = pacman.transform.position;

        pacman.gameObject.SetActive(false);
        Instantiate(fakePacman, spawnPos, Quaternion.identity);

        EvalLives();
    }

    private void EvalLives()
    {
        SetLives(lives - 1);
        if (lives >= 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        } else
        {
            Invoke(nameof(GameOver), 1.5f);
        }
    }

    /**
     * Gets called after pacman's death animation, and only if there are no more remaining lives
     * Enables the UI object that shows the game over message, and gives the player a menu/play again option
     */
    private void GameOver()
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.gameObject.SetActive(false);
        }
        pacman.gameObject.SetActive(false);

        gameOver.SetActive(true);
    }

    private void TogglePause()
    {
        if (this.paused)
        {
            Time.timeScale = 1f;
            gamePaused.SetActive(false);
            this.paused = false;
        } else
        {
            Time.timeScale = 0f;
            gamePaused.SetActive(true);
            this.paused = true;
        }
    }
}
//TODO: sound