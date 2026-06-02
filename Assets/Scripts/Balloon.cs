using UnityEngine;

public class Balloon : MonoBehaviour
{
    public GameObject villain;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDestroy()
    {
        var script = villain.GetComponent<Villain>();
        script.Death();
    }
}
