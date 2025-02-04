using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public AudioSource music;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup musicWithEffectsMixer;
    public GameObject gameoverMenu;

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
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
        music.outputAudioMixerGroup = musicWithEffectsMixer;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
        music.outputAudioMixerGroup = musicMixer;
    }

    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
}
