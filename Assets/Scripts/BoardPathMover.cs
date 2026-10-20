using System;
using UnityEngine;

public class BoardPathMover : MonoBehaviour
{
    [SerializeField] private BoardPath boardPath;
    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private int startingWaypointIndex;
    private int currentWaypointIndex;
    private int targetWaypointIndex;
    private int remainingSteps;
    private bool isMoving;

    public int CurrentWaypointIndex => currentWaypointIndex;


    public event Action OnMovementComplete;


    private void Start()
    {
        if (boardPath == null || boardPath.WaypointCount == 0)
            return;

        currentWaypointIndex = Mathf.Clamp(
            startingWaypointIndex,
            0,
            boardPath.WaypointCount - 1
        );

        Transform startWaypoint = boardPath.GetWaypoint(currentWaypointIndex);

        if (startWaypoint != null)
            transform.position = startWaypoint.position;
    }

    private void Update()
    {
        if (!isMoving)
            return;

        Transform target = boardPath.GetWaypoint(targetWaypointIndex);

        if (target == null)
        {
            isMoving = false;
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            transform.position = target.position;
            currentWaypointIndex = targetWaypointIndex;
            isMoving = false;
            if (remainingSteps > 0)
            {
                MoveToNextWaypoint();
            }
            else
            {
                OnMovementComplete?.Invoke();
            }
        }    
    }

    public void MoveToNextWaypoint()
    {
        if (isMoving || boardPath == null || boardPath.WaypointCount == 0)
            return;

        if (remainingSteps <= 0)
            return;

        targetWaypointIndex =
            (currentWaypointIndex + 1) % boardPath.WaypointCount;

        remainingSteps--;
        isMoving = true;
    }

    public void MoveSteps(int steps)
    {
        if (isMoving || boardPath == null || steps <= 0)
            return;

        remainingSteps = steps;
        MoveToNextWaypoint();
    }
}