using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip eatSound;
    [SerializeField] public AudioClip hurtSound;
    [Header("Snake Settings")]
    [SerializeField] public Transform segmentPrefab;
    [SerializeField] public int initialSize = 3;
    [SerializeField] public PlayerInput playerInput;
    [Header("UI")]
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public TMP_Text highScoreText;
    [SerializeField] public ParticleSystem newHighScoreEffect;
    [Header("Localization")]
    [SerializeField] public LocalizedString scoreString;
    [SerializeField] public LocalizedString highScoreString;
    [Header("Menus")]
    [SerializeField] public PauseMenu pauseMenu;
    [SerializeField] public GameOverMenu gameOverMenu;
    [Header("Food")]
    [SerializeField] public Food food;

    private Vector2 direction = Vector2.right;
    private List<Transform> segments;
    private bool isStopped = false;
    private int _score = 0;
    private int score
    {
        get { return _score; }
        set
        {
            _score = value;
            UpdateScore();
        }
    }
    private int _highScore;
    private int highScore
    {
        get { return _highScore; }
        set
        {
            _highScore = value;
            UpdateHighScore();
        }
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    void Start()
    {
        segments = new List<Transform>();
        segments.Add(this.transform);
        for (int i = 1; i < initialSize; i++)
        {
            segments.Add(Instantiate(segmentPrefab));
        }
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        Time.timeScale = 1;
        score = 0;
    }

    void Update()
    {
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        
        if (playerInput.actions["Pause"].triggered)
        {
            pauseMenu.TogglePause();
        }
        
        if (Time.timeScale == 0)
        {
            return;
        }
        
        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            if (moveInput.x > 0 && direction != Vector2.left)
            {
                direction = Vector2.right;
            }
            else if (moveInput.x < 0 && direction != Vector2.right)
            {
                direction = Vector2.left;
            }
        }
        else
        {
            if (moveInput.y < 0 && direction != Vector2.up)
            {
                direction = Vector2.down;
            }
            else if (moveInput.y > 0 && direction != Vector2.down)
            {
                direction = Vector2.up;
            }
        }
    }

    void FixedUpdate()
    {
        if (isStopped) return;

        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + direction.x,
            Mathf.Round(this.transform.position.y) + direction.y,
            0.0f    
        );
    }

    void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
        score += 100;
        Time.timeScale += 0.02f;
        PlaySound(eatSound);
    }

    public void Reset()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(this.transform);
        score = 0;

        for (int i = 1; i < initialSize; i++)
        {
            segments.Add(Instantiate(segmentPrefab));
        }

        this.transform.position = Vector3.zero;
        Time.timeScale = 1;
        food.RandomPosition();
        isStopped = false;
    }

    void Stop()
    {
        isStopped = true;
        PlaySound(hurtSound);
        if (score > highScore)
        {
            highScore = score;
            newHighScoreEffect.Play();
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            gameOverMenu.Show(true);
        }  else {
            gameOverMenu.Show(false);
        }
    }

    async void UpdateScore()
    {
        AsyncOperationHandle<string> handle = scoreString.GetLocalizedStringAsync();
        await handle.Task;
        scoreText.text = string.Format(handle.Result, score);
    }

    async void UpdateHighScore()
    {
        AsyncOperationHandle<string> handle = highScoreString.GetLocalizedStringAsync();
        await handle.Task;
        highScoreText.text = string.Format(handle.Result, highScore);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
        }
        if (other.tag == "Wall" || other.tag == "Snake")
        {
            Stop();
        }
    }
}
