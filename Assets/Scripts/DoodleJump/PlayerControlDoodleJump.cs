using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControlDoodleJump : MonoBehaviour
{
    [SerializeField] private UI_Controller Ui_Controller;
    [SerializeField] private SpawnerPlatforms Spawnerplatforms;
    [SerializeField] private float force;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Sprite With_Legs, Without_Legs;
    private Camera cam;
    private bool flag_of_winning;
    private float rightBorder;
    private float leftBorder;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;

        cam = Camera.main;
        rightBorder = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, -cam.transform.position.z)).x;
        leftBorder = cam.ScreenToWorldPoint(new Vector3(0, 0, -cam.transform.position.z)).x;

        rb.bodyType = RigidbodyType2D.Static;
    }

    private void Update()
    {
        if (!flag_of_winning && Input.GetMouseButtonDown(0) && Ui_Controller.ClickAnywhere.IsActive())
        {
            flag_of_winning = !flag_of_winning;
            Ui_Controller.StartGame();
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        if (flag_of_winning)
        {
            if (transform.position.y > cam.transform.position.y)
            {
                cam.transform.position = new Vector3(0, transform.position.y, cam.transform.position.z);
                Ui_Controller.ScoreIncrease(1);
            }

            if (Input.mousePosition.x > Screen.width / 2 && Input.GetMouseButton(0))
            {
                rb.AddForce(Vector2.right * force);
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (Input.GetMouseButton(0))
            {
                rb.AddForce(Vector2.left * force);
                GetComponent<SpriteRenderer>().flipX = false;
            }

            if (transform.position.x > rightBorder)
                transform.Translate(Vector3.left * (rightBorder * 2 + 0.1f));
            else if (transform.position.x < leftBorder)
                transform.Translate(Vector3.right * (rightBorder * 2 - 0.1f));
        }

        if (Input.GetKey(KeyCode.Escape))
            Ui_Controller.Exit();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.velocity.y <= 0)
            StartCoroutine(AnimateSprites());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        flag_of_winning = !flag_of_winning;
        rb.bodyType = RigidbodyType2D.Static;
        Ui_Controller.RollWinLosePanel(1);
    }

    private IEnumerator AnimateSprites()
    {
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        GetComponent<SpriteRenderer>().sprite = Without_Legs;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = With_Legs;
    }
}
