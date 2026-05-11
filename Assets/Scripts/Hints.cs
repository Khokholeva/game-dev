using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hints : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            var text = GetComponent<TextMeshProUGUI>();
            text.enabled = !text.isActiveAndEnabled;
        }
    }
}
