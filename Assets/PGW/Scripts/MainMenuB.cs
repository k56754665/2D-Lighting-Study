using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuB : MonoBehaviour
{
    public void GotoStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("BuildB");
    }


    public void QuitGame()
    {
        Application.Quit();
    }


}
