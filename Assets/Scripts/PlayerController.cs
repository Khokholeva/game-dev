using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector3 start = new Vector3(-20f, -3f, 0);
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
    public LinkedList<GameObject> spawnedShapes;

    public GameObject[] bwVariants;
    public Vector3[] moveCamera;
    public GameObject ui;
    private PauseMenu pauseMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = start;
        rb = GetComponent<Rigidbody2D>();
        spawnZone.SetActive(false);
        unlockedColors = new bool[colors.Length];
        unlockedColors[0] = true;
        spawnedShapes = new LinkedList<GameObject>();
        pauseMenu = ui.GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!freezeControls)
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
                        spawnedShapes.AddLast(lastShape);
                        lastShape.GetComponent<SpriteRenderer>().color = colors[colorIndex];
                        lastShape.GetComponent<ObjectScript>().objectList = spawnedShapes;
                        if (spawnedShapes.Count > maxCount)
                        {
                            Destroy(spawnedShapes.First.Value);
                            spawnedShapes.RemoveFirst();
                        }
                        switch (colorIndex)
                        {
                            case 1:
                                lastShape.GetComponent<Rigidbody2D>().mass = 15f; break;
                            case 2:
                                lastShape.AddComponent<Floating>(); break;
                            case 3:
                                lastShape.AddComponent<Explosive>(); break;
                            case 4:
                                lastShape.tag = keyTags[shapeIndex]; break;
                            case 5:
                                lastShape.AddComponent<BouncyScript>();
                                lastShape.GetComponent<Rigidbody2D>().mass = 40f;
                                switch (shapeIndex)
                                {
                                    case 0:
                                        lastShape.GetComponent<BouncyScript>().baseDirection = new Vector2(0, 1f);
                                        break;
                                    case 1:
                                        lastShape.GetComponent<BouncyScript>().baseDirection = new Vector2(2f, 1.2f);
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
                pauseMenu.ChooseShape(shapeIndex);
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
                pauseMenu.ChooseColor(colorIndex);
                if (currentPreview != null)
                {
                    var color = colors[colorIndex];
                    color.a = 0.5f;
                    currentPreview.GetComponent<SpriteRenderer>().color = color;

                }
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

            if (dir != 0)
            {
                transform.localScale = new Vector3(-1 * dir, 1, 1);
            }
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
            var color = (int)collision.transform.localScale.z;
            unlockedColors[color] = true;
            pauseMenu.UnlockColor(color - 1);
            StartCoroutine(UnlockColorCutscene(color - 1));
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("KillZone"))
            rb.position = spawnPosition;
        if (collision.gameObject.CompareTag("SpawnZone"))
            spawnPosition = collision.transform.position;
    }

    IEnumerator UnlockColorCutscene(int color)
    {
        var bwObjects = bwVariants[color];
        var moveVector = moveCamera[color];
        var moved = new Vector3(0, 0, 0);
       
        freezeControls = true;
        
        while ((moved - moveVector).magnitude > 0.5)
        {
            Camera.main.transform.position += (moveVector).normalized * Time.deltaTime * 10;
            moved += (moveVector).normalized * Time.deltaTime * 10;
            if (Camera.main.orthographicSize < 10)
                Camera.main.orthographicSize += Time.deltaTime * 5;
            yield return new WaitForEndOfFrame();
        }
        while (Camera.main.orthographicSize < 10)
        {
            Camera.main.orthographicSize += Time.deltaTime * 5;
            yield return new WaitForEndOfFrame();
        }

        // This pauses ExecuteSequence until LongTask completely finishes
        foreach (Transform child in bwObjects.transform)
        {
            child.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }

        var startPos = transform.position + new Vector3(0, 1, -10);
        while ((Camera.main.transform.position - startPos).magnitude > 0.5)
        {
            Camera.main.transform.position += (startPos - Camera.main.transform.position).normalized * Time.deltaTime * 10;
            if (Camera.main.orthographicSize > 6)
                Camera.main.orthographicSize -= Time.deltaTime * 5;
            yield return new WaitForEndOfFrame();
        }
        Camera.main.transform.position = startPos;

        while (Camera.main.orthographicSize > 6)
        {
            Camera.main.orthographicSize -= Time.deltaTime * 5;
            yield return new WaitForEndOfFrame();
        }
        Camera.main.orthographicSize = 6;
        freezeControls = false;
    }


}
