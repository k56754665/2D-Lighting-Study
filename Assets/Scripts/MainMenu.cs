using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GotoStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("BuildA");
    }


    public void QuitGame()
    {
        Application.Quit();
    }


}
