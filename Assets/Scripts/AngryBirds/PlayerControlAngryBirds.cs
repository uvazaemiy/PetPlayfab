using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControlAngryBirds : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private UI_Controller Ui_Controller;
    [SerializeField] private Transform Shoot;
    [SerializeField] private float maxDist = 3;
    [Space]
    public List<GameObject> Obstacles;
    [Space]
    private Rigidbody2D rb, ShootRb;
    private Joint2D joint;
    private bool isPressed, pause;
    [SerializeField] private Text timer;
    [SerializeField] private int seconds = 10, milliseconds = 0;
    [Space]
    [SerializeField] private List<Transform> PropsToMove;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;        
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        ShootRb = Shoot.GetComponent<Rigidbody2D>();
        joint = GetComponent<Joint2D>();
        timer.text = seconds + ":" + milliseconds;
        StartCoroutine(PredTimer());
    }

    private void Update()
    {
        if (isPressed && !pause)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(mousePos, Shoot.position) > maxDist)
                rb.position = ShootRb.position + (mousePos - ShootRb.position).normalized * maxDist;
            else
                rb.position = mousePos;
        }
        else
            if (transform.position.x > Shoot.position.x + 0.1f)
                joint.enabled = false;

        if (!pause && (Obstacles.Count == 0 || seconds == 0 && milliseconds == 0))
        {
            pause = true;
            StopAllCoroutines();
            Ui_Controller.RollWinLosePanel(1);
        }

        if (Input.GetKey(KeyCode.Escape))
            Ui_Controller.Exit();

    }

    private void OnMouseDown()
    {
        isPressed = true;
        rb.isKinematic = true;
        if (!pause)
            Ui_Controller.StartGame();
    }

    private void OnMouseUp()
    {
        isPressed = false;
        rb.isKinematic = false;
        StartCoroutine(NextBird());
    }

    private IEnumerator NextBird()
    {
        yield return new WaitForSeconds(3);
        transform.position = Shoot.position;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        joint.enabled = true;
        rb.isKinematic = true;
    }

    public void RemoveObstacle(GameObject obj, int score)
    {
        if (!pause)
            Ui_Controller.ScoreIncrease(score);
        Obstacles.Remove(obj);
        Destroy(obj);
    }

    private IEnumerator PredTimer()
    {
        yield return new WaitForSeconds(1);

        foreach (Transform temp in PropsToMove)
            temp.position += Vector3.left * (((float)Screen.width / (float)Screen.height - 16f / 9f) * 15);

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.1f);
        if (milliseconds == 0)
        {
            seconds--;
            milliseconds = 10;
        }
        milliseconds--;
        timer.text = seconds + ":" + milliseconds;
        StartCoroutine(Timer());
    }
}
