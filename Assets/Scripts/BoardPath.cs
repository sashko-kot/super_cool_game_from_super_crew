using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardPath : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    public int WaypointCount => waypoints.Length;

    public Transform GetWaypoint(int index)
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("BoardPath: waypoints are not assigned.", this);
            return null;
        }

        int wrappedIndex = ((index % waypoints.Length) + waypoints.Length) % waypoints.Length;
        return waypoints[wrappedIndex];
    }

    private void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Gizmos.color = Color.white; // Устанавливаем цвет
        Vector3 offset = new Vector3(0, 5, 0);

        // Рисуем точки (сферы)
        foreach (Transform waypoint in waypoints)
        {
            if (waypoint == null)
                continue;

            Gizmos.DrawSphere(waypoint.position + offset, 0.5f);
        }

        // Соединяем соседние точки
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] == null || waypoints[i + 1] == null)
                continue;

            Gizmos.DrawLine(
                waypoints[i].position + offset,
                waypoints[i + 1].position + offset
            );
        }

        // Замыкаем маршрут
        Transform first = waypoints[0];
        Transform last = waypoints[waypoints.Length - 1];

        if (first != null && last != null && waypoints.Length > 1)
        {
            Gizmos.DrawLine(
                last.position + offset,
                first.position + offset
            );
        }
    }

}