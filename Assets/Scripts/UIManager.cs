using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject[] lives;
    public GameObject scoreValue;
    public GameObject attackDisplay;
    private TextMeshProUGUI score;
    
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
        
    }
}
