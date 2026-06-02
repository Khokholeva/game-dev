using UnityEngine;
using System.Collections;

public class ShowVillain : MonoBehaviour
{
    public GameObject villain;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.CompareTag("Player"))
            StartCoroutine(Show(collision.gameObject));

    }

    IEnumerator Show(GameObject player)
    {
        var script = player.GetComponent<PlayerController>();
        script.freezeControls = true;
        
        var newPos = villain.transform.position + new Vector3(0, 1, -10);
        while ((Camera.main.transform.position - newPos).magnitude > 0.5)
        {
            Camera.main.transform.position += (newPos - Camera.main.transform.position).normalized * Time.deltaTime * 10;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1);
        var pos = player.transform.position + new Vector3(0, 1, -10);
        while ((Camera.main.transform.position - pos).magnitude > 0.5)
        {
            Camera.main.transform.position += (pos - Camera.main.transform.position).normalized * Time.deltaTime * 10;
            yield return new WaitForEndOfFrame();
        }
        Camera.main.transform.position = pos;
        script.freezeControls = false;
        Destroy(gameObject);
    }
}
