using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverMenu : MonoBehaviour
{
    public AudioSource music;
    public GameObject newhighscore;
    public Snake snake;
    public GameObject firstElement;

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

    public void Show(bool highscore)
    {
        if (highscore)
        {
            newhighscore.SetActive(true);
        }
        else
        {
            newhighscore.SetActive(false);
        }
        music.Stop();
        this.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstElement);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
