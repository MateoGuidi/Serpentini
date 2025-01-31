using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Snake : MonoBehaviour
{
    [Header("Snake Settings")]
    [SerializeField] public Transform segmentPrefab;
    [SerializeField] public int initialSize = 3;
    [Header("UI")]
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public TMP_Text highScoreText;

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

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        segments = new List<Transform>();
        segments.Add(this.transform);
        for (int i = 1; i < initialSize; i++)
        {
            segments.Add(Instantiate(segmentPrefab));
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) && direction != Vector2.left)
        {
            direction = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && direction != Vector2.up)
        {
            direction = -Vector2.up;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && direction != Vector2.right)
        {
            direction = -Vector2.right;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && direction != Vector2.down)
        {
            direction = Vector2.up;
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
    }

    void Reset()
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
        isStopped = false;
    }

    void Stop()
    {
        isStopped = true;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        //Show Game Over Screen
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateHighScore()
    {
        highScoreText.text = "High Score: " + highScore;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
        }
        if (other.tag == "Wall" || other.tag == "Snake")
        {
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
            } // Tmp (to remove in the future)
            Reset(); // Tmp (should be Stop in the future)
        }
    } 
}
