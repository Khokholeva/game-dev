using UnityEngine;

public class FlowerPot : MonoBehaviour
{
    public GameObject collectible;
    public int dropletCounter = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.CompareTag("Droplet"))
        {
            dropletCounter -= 1;
            if (dropletCounter <= 0)
            {
                var animator = gameObject.GetComponent<Animator>();
                animator.SetBool("isWater", true);
                collectible.SetActive(true);
            }
        }
    }
}
