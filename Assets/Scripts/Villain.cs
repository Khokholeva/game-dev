using System.Collections;
using UnityEngine;

public class Villain : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        
        StartCoroutine(DeathCutscene());
    }

    IEnumerator DeathCutscene()
    {
        var anim = GetComponent<Animator>();
        anim.SetBool("BallonDestroyed", true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
