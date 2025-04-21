using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    // �޴����� �����ϴ� ��ư�� 
    // «������ �����ϴ� ��ũ��Ʈ�Դϴ�.

    public void GameStart()
    {
        // ��ư Ŭ�� ��, ���� ������ ��ȯ
        SceneManager.LoadScene("Build");
    }

    public void GoToGuide()
    {
        // ��ư Ŭ�� ��, ���̵�(How to Play) ������ ��ȯ
        SceneManager.LoadScene("Guide");
    }

    public void BackToMain()
    {
        // ��ư Ŭ�� ��, ���� Ÿ��Ʋ ������ ��ȯ
        SceneManager.LoadScene("MainTitle");
    }

    public void GameExit()
    {
        // ��ư Ŭ�� ��, ���� ����
        Application.Quit();
    }
}
