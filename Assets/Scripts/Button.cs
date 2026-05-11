using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Button : MonoBehaviour
{
    public GameObject[] connectedObjects;
    public GameObject[] animatedObjects;
    private bool state = false;
    public GameObject tooltip;
    private bool playerClose = false;
    public string firstTooltip = "(q) Open";
    public string secondTooltip = "(q) Close";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tooltip.SetActive(false);
        tooltip.GetComponent<TextMeshPro>().text = firstTooltip;
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerClose && Keyboard.current.qKey.wasPressedThisFrame)
        {
            foreach(var obj in connectedObjects)
                obj.SetActive(!obj.activeSelf);
            foreach(var obj in animatedObjects)
            {
                var animator = obj.GetComponent<Animator>();
                animator.SetBool("isOpen", !animator.GetBool("isOpen"));
            }
            state = !state;
            if (state)
            {
                GetComponent<SpriteRenderer>().color = Color.green;
                tooltip.GetComponent<TextMeshPro>().text = secondTooltip;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                tooltip.GetComponent<TextMeshPro>().text = firstTooltip;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tooltip.SetActive(true);
            playerClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tooltip.SetActive(false);
            playerClose = false;
        }
    }
}
