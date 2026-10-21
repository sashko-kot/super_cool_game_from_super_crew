using UnityEngine;

public class BoardCell : MonoBehaviour
{
    [SerializeField] private BoardCell[] nextCells;

    public Transform CellTransform => transform;
    public int NextCellCount => nextCells == null ? 0 : nextCells.Length;


    public BoardCell GetNextCell(int index)
    {
        if (nextCells == null || index < 0 || index >= nextCells.Length)
        {
            Debug.LogError("BoardCell: invalid next cell index.", this);
            return null;
        }

        if (nextCells[index] == null)
        {
            Debug.LogError(
                $"BoardCell: next cell at index {index} is not assigned.",
                this
            );
            return null;
        }

        return nextCells[index];
    }

    public bool HasMultipleNextCells()
    {
        return NextCellCount > 1;
    }

    private void OnDrawGizmos() //Визуализация маршрута
    {

        Gizmos.color = Color.white; // Устанавливаем цвет
        Vector3 offset = new Vector3(0, 1, 0);

        // Рисуем точки (сферы)
        Gizmos.DrawSphere(CellTransform.position + offset, 0.2f);

        // Соединяем соседние точки
        foreach (BoardCell boardCell in nextCells)
        {
            if (boardCell == null || nextCells.Length == 0)
                continue;

            Gizmos.DrawLine(
                CellTransform.position + offset,
                boardCell.CellTransform.position + offset
            );
        }

    }

}