using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject quitMenu;

    // UI
    public GameObject mainPlayBtn;
    public GameObject optionsBackBtn;
    public GameObject quitBackBtn;

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ToggleOptionsMenu()
    {
        if (optionsMenu.activeSelf)
        {
            optionsMenu.SetActive(false);
            this.gameObject.SetActive(true);

            SetActiveButton(mainPlayBtn);
        }
        else
        {
            this.gameObject.SetActive(false);
            optionsMenu.SetActive(true);

            SetActiveButton(optionsBackBtn);
        }
    }

    public void ToggleQuitMenu()
    {
        if (quitMenu.activeSelf)
        {
            quitMenu.SetActive(false);
            this.gameObject.SetActive(true);

            SetActiveButton(mainPlayBtn);
        } else
        {
            this.gameObject.SetActive(false);
            quitMenu.SetActive(true);

            SetActiveButton(quitBackBtn);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void SetActiveButton(GameObject input)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(input);
    }
}
