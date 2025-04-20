using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    // 메뉴들이 동작하는 버튼을 
    // 짬뽕으로 관리하는 스크립트입니다.

    public void GameStart()
    {
        // 버튼 클릭 시, 게임 씬으로 전환
        SceneManager.LoadScene("Build");
    }

    public void GoToGuide()
    {
        // 버튼 클릭 시, 가이드(How to Play) 씬으로 전환
        SceneManager.LoadScene("Guide");
    }

    public void BackToMain()
    {
        // 버튼 클릭 시, 메인 타이틀 씬으로 전환
        SceneManager.LoadScene("MainTitle");
    }

    public void GameExit()
    {
        // 버튼 클릭 시, 게임 종료
        Application.Quit();
    }
}
