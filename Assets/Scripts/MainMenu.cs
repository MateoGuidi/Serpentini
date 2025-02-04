using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Settings");
    }
}
