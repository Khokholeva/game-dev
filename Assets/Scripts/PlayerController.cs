using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;
    public float jumpForce = 5.0f;
    private Rigidbody2D rb;
    public GameObject[] shapePreviews;
    public GameObject[] shapes;
    public int index = 0;
    public Vector3 mousePosition;
    public bool spawnState = false;
    public GameObject currentPreview;
    public GameObject spawnZone;
    public float spawnRadius = 15.0f;
    public float dist;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnZone.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
        if (spawnState)
        {
            dist = (currentPreview.transform.position - transform.position).magnitude;
            if (dist > spawnRadius)
            {
                spawnZone.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                spawnZone.GetComponent<Renderer>().material.color = Color.green;
            }
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!spawnState)
            {
                mousePosition = Mouse.current.position.ReadValue();
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                currentPreview = Instantiate(shapePreviews[index], mousePosition, new Quaternion(), this.transform);
                spawnState = true;
                spawnZone.SetActive(true);
            }
            else
            {
                var dist = (currentPreview.transform.position - transform.position).magnitude;
                if (dist <= spawnRadius)
                {
                    Instantiate(shapes[index], currentPreview.transform.position, new Quaternion());
                    Destroy(currentPreview);
                    spawnZone.SetActive(false);
                    spawnState = false;
                }
            }
        }
        if (Mouse.current.rightButton.wasPressedThisFrame && spawnState)
        {
            Destroy(currentPreview);
            spawnZone.SetActive(false);
            spawnState = false;
        }
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            index = (index + 1) % shapePreviews.Length;
        }
    }

    private void FixedUpdate()
    {
        float dir = 0;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            dir = -1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            dir = 1;

        rb.linearVelocityX = speed * dir;
        
    }
}
