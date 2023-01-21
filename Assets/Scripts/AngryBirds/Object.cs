using UnityEngine;

public class Object : MonoBehaviour
{
    public int stamina = 5;
    [SerializeField] private PlayerControlAngryBirds Player;

    private void Start()
    {
        Player.Obstacles.Add(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > stamina)
        {
            Player.RemoveObstacle(gameObject, stamina);
        }
    }
}
