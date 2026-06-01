using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    public GameObject droplet;
    public float interval = 0.5f;
    float timer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            timer = 0;
            Instantiate(droplet, this.transform.position, new Quaternion());
        }
        
    }
}
