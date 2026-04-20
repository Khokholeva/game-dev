using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;
    public float jumpForce = 5.0f;
    private Rigidbody2D rb;
    public bool grounded = true;

    public GameObject[] shapePreviews;
    public GameObject[] shapes;
    public Color[] colors;
    public bool[] unlockedColors;
    public int shapeIndex = 0;
    public int colorIndex = 0;

    public Vector3 mousePosition;
    public bool spawnState = false;
    public GameObject currentPreview;
    public GameObject lastShape;

    public GameObject spawnZone;
    public float spawnRadius = 15.0f;
    public float dist;

    public float floatingSpeed = 1;

    Preview previewScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnZone.SetActive(false);
        unlockedColors = new bool[colors.Length];
        unlockedColors[0] = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && grounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
        if (spawnState)
        {
            dist = (currentPreview.transform.position - transform.position).magnitude;
            if (dist > spawnRadius || previewScript.collisionCounter > 0)
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
                currentPreview = Instantiate(shapePreviews[shapeIndex], mousePosition, new Quaternion(), this.transform);
                currentPreview.GetComponent<Renderer>().material.color = colors[colorIndex];
                previewScript = currentPreview.GetComponent<Preview>();
                spawnState = true;
                spawnZone.SetActive(true);
            }
            else
            {
                var dist = (currentPreview.transform.position - transform.position).magnitude;
                if (dist <= spawnRadius && previewScript.collisionCounter == 0)
                {
                    lastShape = Instantiate(shapes[shapeIndex], currentPreview.transform.position, new Quaternion());
                    lastShape.GetComponent<Renderer>().material.color = colors[colorIndex];
                    switch (colorIndex)
                    {
                        case 1:
                            lastShape.AddComponent<BouncyScript>();
                            break;
                        case 2:
                            lastShape.GetComponent<Rigidbody2D>().gravityScale = -0.6f;
                            break;
                        case 3:
                            lastShape.GetComponent<Rigidbody2D>().mass = 20f; break;
                    }
                    
                    Destroy(currentPreview);
                    currentPreview = null;
                    spawnZone.SetActive(false);
                    spawnState = false;
                }
            }
        }
        if (Mouse.current.rightButton.wasPressedThisFrame && spawnState)
        {
            Destroy(currentPreview);
            currentPreview = null;
            spawnZone.SetActive(false);
            spawnState = false;
        }
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            shapeIndex = (shapeIndex + 1) % shapePreviews.Length;
            if (currentPreview != null)
            {
                Destroy(currentPreview);
                mousePosition = Mouse.current.position.ReadValue();
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                currentPreview = Instantiate(shapePreviews[shapeIndex], mousePosition, new Quaternion(), this.transform);
                currentPreview.GetComponent<Renderer>().material.color = colors[colorIndex];
                previewScript = currentPreview.GetComponent<Preview>();
            }
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            colorIndex = (colorIndex + 1) % colors.Length;
            while (!unlockedColors[colorIndex]) colorIndex = (colorIndex + 1) % colors.Length;
            if (currentPreview != null)
            {
                currentPreview.GetComponent<Renderer>().material.color = colors[colorIndex];
            }
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
        grounded = Physics2D.BoxCast(transform.position, new Vector2(1f, 1f), 0f, Vector2.down, 0.05f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.CompareTag("ColorUnlock"))
        {
            unlockedColors[(int)collision.transform.localScale.z] = true;
            Destroy(collision.gameObject);
        }
    }
}
