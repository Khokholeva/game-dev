using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float baseTimer = 0.3f;
    private float timer = 0f;
    private int countdown = 5;
    private float explosionRadius = 2f;
    private bool exploded = false;
    private GameObject circle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (exploded) {
            if (timer > 0.1f) {
                Destroy(circle);
                Destroy(gameObject);
            }
        }
        else if (timer > baseTimer)
        {
            timer = 0;
            countdown--;
            if (countdown < 1)
            {
                exploded = true;
                circle = new GameObject("Explosion");
                circle.transform.position = transform.position;
                SpriteRenderer renderer = circle.AddComponent<SpriteRenderer>();
                circle.transform.localScale = new Vector3(4, 4, 4);
                renderer.sprite = Resources.Load<Sprite>("Explosion");
                renderer.color = new Color(1f, 1f, 1f);
                renderer.sortingOrder = 20;
                
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject.CompareTag("FragilePlatform"))
                    {
                       
                        Destroy(hitCollider.gameObject);
                    }
                }
            }
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f / (countdown + 1), 1f / (countdown + 1));
            
        }
    }
}
