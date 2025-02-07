using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public AudioSource music;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup musicWithEffectsMixer;
    public GameObject gameoverMenu;
    public GameObject firstElement;

    private float savedTimeScale;

    public void TogglePause()
    {
        if (gameoverMenu.activeSelf)
        {
            return;
        }
        if (Time.timeScale == 0)
        {
            Resume();
        } else 
        {
            Pause();
        }
    }

    public void Pause()
    {
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
        music.outputAudioMixerGroup = musicWithEffectsMixer;
        EventSystem.current.SetSelectedGameObject(firstElement);
    }

    public void Resume()
    {
        Time.timeScale = savedTimeScale;
        this.gameObject.SetActive(false);
        music.outputAudioMixerGroup = musicMixer;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
}
