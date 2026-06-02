using UnityEngine;
using System.Collections;

public class ExplosiveCutscene : MonoBehaviour
{
    public GameObject cutsceneSprite;
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
        {
            StartCoroutine(Cutscene(collision.gameObject));
        }
    }

    IEnumerator Cutscene(GameObject player)
    {
        var script = player.GetComponent<PlayerController>();
        var anim = cutsceneSprite.GetComponent<Animator>();
        anim.SetBool("Play", true);
        script.freezeControls = true;
        yield return new WaitForSeconds(5);
        script.freezeControls = false;
        cutsceneSprite.SetActive(false);
    }
}
