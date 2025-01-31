using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField]
    public BoxCollider2D border;

    void Start()
    {
        RandomPosition();
    }

    void RandomPosition()
    {
        Bounds bounds = border.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(
            Mathf.Round(x),
            Mathf.Round(y),
            0.0f
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Snake")
        {
            RandomPosition();
        }
    }   

}
