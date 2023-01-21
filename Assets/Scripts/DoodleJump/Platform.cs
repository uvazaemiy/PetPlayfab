using UnityEngine;

public class Platform : MonoBehaviour
{
    public int id;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "LoseArea")
        {
            transform.position = new Vector3(Random.Range(cam.ScreenToWorldPoint(new Vector3(0, 0, -cam.transform.position.z)).x + transform.localScale.x / 4, cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, -cam.transform.position.z)).x - transform.localScale.x / 4), Random.Range(transform.position.y + 10.3f, transform.position.y + 14.3f), 0);
            id++;
        }
    }
}
