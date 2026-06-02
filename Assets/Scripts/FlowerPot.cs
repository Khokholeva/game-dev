using UnityEngine;
using System.Collections;

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
            if (dropletCounter == 0)
            {
                StartCoroutine(Grow());
            }
        }
    }

    IEnumerator Grow()
    {
        var animator = gameObject.GetComponent<Animator>();
        animator.SetBool("IsWater", true);
        yield return new WaitForSeconds(1.5f);
        collectible.SetActive(true);
    }
}
