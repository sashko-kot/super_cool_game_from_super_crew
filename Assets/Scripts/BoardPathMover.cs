using System;
using UnityEngine;

public class BoardPathMover : MonoBehaviour
{

    [Header("Movement On Board")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float acceleration = 3f;
    private float currentSpeed;

    [SerializeField] private BoardCell startingCell;
    private BoardCell currentCell;
    private BoardCell targetCell;

    private int remainingSteps;
    private bool isMoving;
    private bool waitingForBranchChoice;

    private BoardCellView[] highlightedCells; // Для развилок


    public event Action OnMovementComplete;

    #region ТЕСТЫ
    [ContextMenu("Test: Roll The Dice")]
    public void TestRollTheDice()
    {
        int roll = UnityEngine.Random.Range(1, 11);
        Debug.Log("Бросок кубика: " + roll);
        MoveSteps(roll);
    }

    [ContextMenu("Test: 3 steps")]
    public void TestThreeSteps()
    {
        MoveSteps(3);
    }

    [ContextMenu("Test: choose 1 brench")]
    public void TestFirstBranch()
    {
        SelectBranch(0);
    }

    [ContextMenu("Test: choose 2 brench")]
    public void TestSecondBranch()
    {
        SelectBranch(1);
    }
    #endregion

    private void Start()
    {
        if (startingCell == null)
        {
            Debug.LogError("BoardPathMover: starting cell is not assigned.", this);
            return;
        }

        transform.position = startingCell.CellTransform.position;
        currentCell = startingCell;
    }

    private void Update()
    {
        if (!isMoving)
            return;

        if (targetCell == null)
        {
            isMoving = false;
            return;
        }

        Transform target = targetCell.CellTransform;

        if (target == null)
        {
            isMoving = false;
            return;
        }

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            moveSpeed,
            acceleration * Time.deltaTime
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            currentSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            transform.position = target.position;
            currentCell = targetCell;
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
        if (isMoving || currentCell == null)
            return;

        if (waitingForBranchChoice)
            return;

        if (remainingSteps <= 0)
            return;

        if (currentCell.NextCellCount == 0)
        {
            Debug.LogError(
                $"BoardPathMover: {currentCell.name} has no next cells.",
                currentCell
            );

            remainingSteps = 0;
            OnMovementComplete?.Invoke();
            return;
        }

        if (currentCell.HasMultipleNextCells())
        {
            waitingForBranchChoice = true;
            ShowBranchChoices(); // Включает подсветку развилки
            Debug.Log($"Waiting for branch choice at {currentCell.name}", this);
            return;
        }

        targetCell = currentCell.GetNextCell(0);

        if (targetCell == null)
        {
            currentSpeed = 0f;
            remainingSteps = 0;
            OnMovementComplete?.Invoke();
            return;
        }

        remainingSteps--;
        isMoving = true;
    }

    public void MoveSteps(int steps)
    {
        if (isMoving || currentCell == null || steps <= 0)
            return;

        currentSpeed = 0;
        remainingSteps = steps;
        MoveToNextWaypoint();
    }

    public void SelectBranch(int branchIndex)
    {
        if (!waitingForBranchChoice)
            return;

        if (branchIndex < 0 || branchIndex >= currentCell.NextCellCount)
        {
            Debug.LogWarning("Invalid branch index.", this);
            return;
        }

        waitingForBranchChoice = false;
        ClearBranchChoices(); // Выключает подсвтку развилки
        targetCell = currentCell.GetNextCell(branchIndex);

        remainingSteps--;
        isMoving = true;
    }

    public void TrySelectBranch(BoardCell selectedCell)
    {
        if (!waitingForBranchChoice || selectedCell == null)
            return;

        for (int i = 0; i < currentCell.NextCellCount; i++)
        {
            if (currentCell.GetNextCell(i) == selectedCell)
            {
                SelectBranch(i);
                return;
            }
        }
    }

    #region ПОДСВЕТКА РАЗВИЛКИ
    private void ShowBranchChoices()
    {
        highlightedCells = new BoardCellView[currentCell.NextCellCount];

        for (int i = 0; i < currentCell.NextCellCount; i++)
        {
            BoardCell nextCell = currentCell.GetNextCell(i);
            BoardCellView view = nextCell.GetComponentInChildren<BoardCellView>();

            if (view != null)
            {
                highlightedCells[i] = view;
                view.SetHighlighted(true);
            }
        }
    }

    private void ClearBranchChoices()
    {
        if (highlightedCells == null)
            return;

        foreach (BoardCellView view in highlightedCells)
        {
            if (view != null)
                view.SetHighlighted(false);
        }

        highlightedCells = null;
    }
    #endregion


}