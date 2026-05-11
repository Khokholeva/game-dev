using UnityEngine;

public class CageCutscene : MonoBehaviour
{
    public GameObject cage;
    PlayerController playerController;
    GameObject player;
    public Vector3 secondStop;
    public Vector3 firstStop;
    public float speed;
    public bool moving = false;
    public bool returning = false;
    float totalTime;
    float timer;
    Vector3 moveVector;

    public Sprite closedSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (timer < totalTime)
                cage.transform.position += moveVector.normalized * speed * Time.deltaTime;
            else
            {
                //player.transform.position += new Vector3(0, 1f, 0);
                cage.GetComponent<SpriteRenderer>().sprite = closedSprite;
                cage.GetComponent<Collider2D>().enabled = true;
                moving = false;
                returning = true;
                totalTime = (secondStop - cage.transform.position).magnitude / speed;
                timer = 0;
                moveVector = (secondStop - cage.transform.position).normalized;
            }
            timer += Time.deltaTime;
        }
        if (returning)
        {
            if (timer < totalTime)
                cage.transform.position += moveVector.normalized * speed * Time.deltaTime;
            else
            {
                playerController.freezeControls = false;
                Destroy(gameObject);
            }
            timer += Time.deltaTime;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            playerController = collision.GetComponent<PlayerController>();
            playerController.freezeControls = true;
            moving = true;
            cage.GetComponent<Collider2D>().enabled = false;
            totalTime = (firstStop - cage.transform.position).magnitude / speed;
            timer = 0;
            moveVector = firstStop - cage.transform.position;
        }
    }
}
