using UnityEngine;

public class BoardMovement : MonoBehaviour
{

    [Header("Карта тайлов")]
    [SerializeField] private TileMap tm;

    private Transform[] waypoints;
    private bool isMoving = false;
    private int move;
    [SerializeField] private float speed = 15f;

    void Start()
    {
        waypoints = tm.getTileMap();
    }

    
    void Update()
    {
 
        if (GameManager.Instance.gameState == GameState.BoardGame && isMoving == true)
        {
            Vector3 offset = new Vector3(0, 12, 0);
            transform.position = Vector3.MoveTowards(transform.position, waypoints[2].position + offset, speed * Time.deltaTime);
            /*
            for (int i = 0; i < move; i++)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoints[i].position + offset, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, waypoints[i].position + offset) > 0.1f) i--;
                else Debug.Log("Положение игрока: " + transform.position);
            }
            */
            if (Vector3.Distance(transform.position, waypoints[2].position + offset) < 0.1f) isMoving = false;
        }
    }

    public void MoveOnBoard(int roll)
    {
        move = roll;
        isMoving = true;
        Debug.Log("Двигаюсь на " + move);
    }
}
