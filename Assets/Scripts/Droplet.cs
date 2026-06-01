using UnityEngine;

public class Droplet : MonoBehaviour
{
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
        if (collision == null) return;
        if (!collision.gameObject.CompareTag("SpawnedObject"))
            Destroy(gameObject);
    }
}
