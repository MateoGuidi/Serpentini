using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public AudioSource music;
    public Snake snake;

    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }

    public void Retry()
    {
        Hide();
        music.Play();
        snake.Reset();
    }

    public void Show()
    {
        music.Stop();
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
