using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject scoreValue;
    public GameObject livesValue;
    public GameObject attackDisplay;
    private TextMeshProUGUI score;
    private TextMeshProUGUI lives;
    
    private static UIManager _instance;
    public static UIManager Instance
    {
        get {
            if (_instance == null)
            {
                Debug.Log("UIManager _instance is null!");
            }

            return _instance;
        }
    }
    // Singleton pattern: private static + public static with custom get/set ensures there will only ever be a single instance of this class.

    public void Awake()
    {
        _instance = this;

        score = scoreValue.GetComponent<TextMeshProUGUI>();
        lives = livesValue.GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore(int newScore)
    {
        score.text = newScore.ToString();
    }

    public void UpdateAttackDisplay(bool canAttack)
    {
        if (canAttack)
        {
            attackDisplay.SetActive(true);
        } else
        {
            attackDisplay.SetActive(false);
        }
    }

    public void UpdateLives(int lives)
    {
        switch(lives)
        {
            case 0:
                this.lives.text = "0x"; 
                break;
            case 1:
                this.lives.text = "1x";
                break;
            case 2:
                this.lives.text = "2x";
                break;
            case 3:
                this.lives.text = "3x";
                break;
            default:
                this.lives.text = "#ERR";
                break;
        }
    }
}
