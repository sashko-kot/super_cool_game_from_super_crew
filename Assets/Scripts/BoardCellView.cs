using UnityEngine;

public class BoardCellView : MonoBehaviour
{
    private BoardCell boardCell;
    private Renderer visualRenderer;
    private Color originalColor;

    public BoardCell BoardCell => boardCell;

    private void Awake()
    {
        boardCell = GetComponentInParent<BoardCell>();
        visualRenderer = GetComponent<Renderer>();

        if (visualRenderer != null)
            originalColor = visualRenderer.material.color;
    }

    private void OnMouseDown()
    {
        if (boardCell == null)
            return;

        BoardPathMover mover = FindAnyObjectByType<BoardPathMover>();

        if (mover != null)
            mover.TrySelectBranch(boardCell);
    }

    public void SetHighlighted(bool highlighted)
    {
        if (visualRenderer == null)
            return;

        visualRenderer.material.color = highlighted
            ? Color.yellow
            : originalColor;
    }

}