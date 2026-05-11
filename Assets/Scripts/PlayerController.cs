using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;
    public float accelerationRate = 1.0f;
    public float jumpForce = 5.0f;
    private Rigidbody2D rb;
    private bool grounded = true;

    public GameObject[] shapePreviews;
    public GameObject[] shapes;
    public string[] keyTags;
    public Color[] colors;
    public bool[] unlockedColors;
    private int shapeIndex = 0;
    private int colorIndex = 0;

    private Vector3 mousePosition;
    private bool spawnState = false;
    private GameObject currentPreview;
    private GameObject lastShape;

    public GameObject spawnZone;
    public float spawnRadius = 15.0f;
    private float dist;

    private Vector2 spawnPosition;

    Preview previewScript;

    public bool freezeControls = false;

    public int maxCount = 10;
    public GameObject[] spawnedShapes;
    public int newShapeIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(-2f, -3f, 0);
        rb = GetComponent<Rigidbody2D>();
        spawnZone.SetActive(false);
        unlockedColors = new bool[colors.Length];
        unlockedColors[0] = true;
        spawnedShapes = new GameObject[maxCount];
    }

    // Update is called once per frame
    void Update()
    {
        if (!freezeControls)
            if (Keyboard.current.spaceKey.wasPressedThisFrame && grounded)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        if (spawnState)
        {
            dist = (currentPreview.transform.position - transform.position).magnitude;
            if (dist > spawnRadius || previewScript.collisionCounter > 0)
            {
                spawnZone.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.25f);
            }
            else
            {
                spawnZone.GetComponent<SpriteRenderer>().color = new Color(0.7f, 1f, 0.7f, 0.25f);
            }
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!spawnState)
            {
                mousePosition = Mouse.current.position.ReadValue();
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                currentPreview = Instantiate(shapePreviews[shapeIndex], mousePosition, new Quaternion(), this.transform);
                var color = colors[colorIndex];
                color.a = 0.4f;
                currentPreview.GetComponent<SpriteRenderer>().color = color;
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
                    if (spawnedShapes[newShapeIndex] != null)
                        Destroy(spawnedShapes[newShapeIndex]);
                    spawnedShapes[newShapeIndex] = lastShape;
                    newShapeIndex = (newShapeIndex + 1) % maxCount;
                    lastShape.GetComponent<SpriteRenderer>().color = colors[colorIndex];
                    switch (colorIndex)
                    {
                        case 1:
                            lastShape.GetComponent<Rigidbody2D>().mass = 100f; break;
                        case 2:
                            lastShape.AddComponent<Floating>(); break;
                        case 3:
                            lastShape.AddComponent<Explosive>(); break;
                        case 4:
                            lastShape.tag = keyTags[shapeIndex]; break;
                        case 5:
                            lastShape.AddComponent<BouncyScript>();
                            lastShape.GetComponent<Rigidbody2D>().mass = 20f;
                            switch (shapeIndex)
                            {
                                case 0:
                                    lastShape.GetComponent<BouncyScript>().baseDirection = new Vector2(0, 1f);
                                    break;
                                case 1:
                                    lastShape.GetComponent<BouncyScript>().baseDirection = new Vector2(2f, 1f);
                                    break;
                                case 2:
                                    lastShape.GetComponent<BouncyScript>().baseDirection = new Vector2(0f, 1f); break;
                            }
                            break;

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
                var color = colors[colorIndex];
                color.a = 0.5f;
                currentPreview.GetComponent<SpriteRenderer>().color = color;
                previewScript = currentPreview.GetComponent<Preview>();
            }
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            colorIndex = (colorIndex + 1) % colors.Length;
            while (!unlockedColors[colorIndex]) colorIndex = (colorIndex + 1) % colors.Length;
            if (currentPreview != null)
            {
                var color = colors[colorIndex];
                color.a = 0.5f;
                currentPreview.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!freezeControls)
        {
            float dir = 0;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                dir = -1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                dir = 1;

            //rb.linearVelocityX = speed * dir;
            float targetSpeed = dir * speed;
            float speedDiff = targetSpeed - rb.linearVelocity.x;
            if (grounded)
                speedDiff *= accelerationRate;
            rb.AddForce(speedDiff * Vector2.right, ForceMode2D.Force);
        }
        else
        {
            rb.linearVelocityX = 0;
        }
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
        if (collision.gameObject.CompareTag("KillZone"))
            rb.position = spawnPosition;
        if (collision.gameObject.CompareTag("SpawnZone"))
            spawnPosition = collision.transform.position;
    }
}
