using UnityEngine;

public class BouncyScript : MonoBehaviour
{
    public float bounceForce = 10.0f;
    public Vector2 baseDirection;
    public Vector2 force;
    
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
        var isLeft = collision.transform.position.x < transform.position.x ? -1 : 1;
        force = new Vector2(baseDirection.x * bounceForce * isLeft, baseDirection.y * bounceForce);
        if (collision.rigidbody != null)
        {
            collision.rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
