using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length, startPos;
    private Transform cam;
    public float parallaxEffect; 

    void Start()
    {
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }

        startPos = transform.position.x;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            length = spriteRenderer.bounds.size.x;
        }
    }

    void LateUpdate()
    {
        if (cam == null) return;

        var mov = (cam.position.x * (1 - parallaxEffect));
        var dist = (cam.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (mov > startPos + length - 8f)
        {
            startPos += length;
        }
        else if (mov < startPos - length + 8f)
        {
            startPos -= length;
        }
    }
}
