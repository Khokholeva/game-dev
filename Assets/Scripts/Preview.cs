using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Preview : MonoBehaviour
{

    private Vector3 mousePosition;
    public float moveSpeed = 0.1f;
    public GameObject objectToSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Mouse.current.position.ReadValue();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
    }
}
