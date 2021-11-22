using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public List<Bomb> bombs = new List<Bomb>();
    public Sprite[] fruitSprites;
    public GameObject fruitPrefab;

    private int totalPellets;
    private int remainingPellets;
    private bool fruitSpawned;
    private GameObject fruit;

    public int explosionBonus = 20;
    public int ghostMultiplier { get; private set; }
    private static int _score { get; set; }
    public static int score
    {
        get { return _score; }
        set {
            _score = value;
            UIManager.Instance.UpdateScore(_score);
        }
    }

    private int _lives;
    public int lives
    {
        get { return _lives; }
        set {
            _lives = value;
            UIManager.Instance.UpdateLives(value);
        }
    }

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
    // Singleton pattern: private static + public static with custom get/set ensures there will only ever be a single instance of this class.

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        totalPellets = pellets.childCount;
        NewGame();
    }

    private void NewGame()
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

    private void GameOver()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
        }

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
        pacman.gameObject.SetActive(false);
        SetLives(lives - 1);
        if (lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        } else
        {
            // TODO: Game over!
            //GameOver();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
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
            // TODO: Victory!
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].vulnerable.Enable(pellet.duration);

            if (ghosts[i].movement.speed - pacman.movement.speed <= 0.3f)
            {
                ghosts[i].scatter.Enable(pellet.duration);
            }
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
            float var = Mathf.SmoothStep(ghost.movement.speed, pacman.movement.speed, 0.3f);
            ghost.movement.speed = var;
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
}
//TODO: sound