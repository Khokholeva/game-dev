using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.InputSystem;

public class Button : MonoBehaviour
{
    public GameObject door;
    private bool state = false;
    public GameObject tooltip;
    private bool playerClose = false;
    public string firstTooltip = "(q) Open";
    public string secondTooltip = "(q) Close";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tooltip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerClose && Keyboard.current.qKey.wasPressedThisFrame)
        {
            door.SetActive(state);
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
