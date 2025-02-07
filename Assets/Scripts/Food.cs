using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField]
    public BoxCollider2D border;

    void Start()
    {
        RandomPosition();
    }

    public void RandomPosition()
    {
        Bounds bounds = border.bounds;
        Vector3 newPosition;

        do
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);

            newPosition = new Vector3(
                Mathf.Round(x),
                Mathf.Round(y),
                0.0f
            );
        } while (IsCollidingWithSnake(newPosition));

        this.transform.position = newPosition;
    }

    private bool IsCollidingWithSnake(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Snake")
            {
                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Snake")
        {
            RandomPosition();
        }
    }   

}