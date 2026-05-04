using UnityEngine;

public class KeyChecker : MonoBehaviour
{
    public string shapeTag;
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
        if (collision.gameObject.CompareTag(shapeTag)) {
            gameObject.SetActive(false);
        }
    }
}
