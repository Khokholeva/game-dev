using UnityEngine;

public class ThinPlatform : MonoBehaviour
{
    public Sprite brokenSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody != null) { 
            if (collision.rigidbody.mass > 50)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = brokenSprite;
                gameObject.GetComponent<Collider2D>().enabled = false;
            }

        }
    }
}
