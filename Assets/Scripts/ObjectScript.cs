using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectScript : MonoBehaviour
{
    public LinkedList<GameObject> objectList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            objectList.Remove(gameObject);
            Destroy(gameObject);
        }
    }
	
	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.CompareTag("KillZone"))
            Destroy(gameObject);
    }
}
