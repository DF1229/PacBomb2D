using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject quitMenu;

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
        }
        else
        {
            this.gameObject.SetActive(false);
            optionsMenu.SetActive(true);
        }
    }

    public void ToggleQuitMenu()
    {
        if (quitMenu.activeSelf)
        {
            quitMenu.SetActive(false);
            this.gameObject.SetActive(true);
        } else
        {
            this.gameObject.SetActive(false);
            quitMenu.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
