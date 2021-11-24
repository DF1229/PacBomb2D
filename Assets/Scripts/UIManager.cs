using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Image life1;
    public Image life2;
    public Image life3;

    public GameObject scoreValue;
    public GameObject attackDisplay;
    private TextMeshProUGUI score;
    private Image[] lives = new Image[3];
    
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

    public void Awake()
    {
        _instance = this;

        score = scoreValue.GetComponent<TextMeshProUGUI>();
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
        switch (lives)
        {
            case 0:
                life1.gameObject.SetActive(false);
                break;
            case 1:
                life2.gameObject.SetActive(false);
                break;
            case 2:
                life3.gameObject.SetActive(false);
                break;
            case 3:
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(true);
                life3.gameObject.SetActive(true);
                break;
            default:
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(false);
                life3.gameObject.SetActive(true);
                Debug.Log("UpdateLives() default case reached");
                break;
        }
    }
}
