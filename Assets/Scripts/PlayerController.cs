using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;
    public float jumpForce = 5.0f;
    private Rigidbody2D rb;
    public GameObject[] shapePreviews;
    public int index = 0;
    public Vector3 mousePosition;
    public bool spawnState = false;
    public GameObject currentPreview;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!spawnState)
            {
                mousePosition = Mouse.current.position.ReadValue();
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                currentPreview = Instantiate(shapePreviews[index], mousePosition, new Quaternion());
                spawnState = true;
            }
            else
            {
                spawnState = false;
            }
        }
        if (Mouse.current.rightButton.wasPressedThisFrame && spawnState)
        {
            spawnState = false;
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

        if (Keyboard.current.eKey.isPressed)
        {
            index = (index + 1) % shapePreviews.Length;
        } 
    }
}
