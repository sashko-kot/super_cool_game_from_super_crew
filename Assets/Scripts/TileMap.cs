using UnityEngine;

public class TileMap : MonoBehaviour
{
    public Transform[] tileMap;

    void Start()
    {
        /*
        tileMap = new Transform[transform.childCount];
        tileMap = gameObject.GetComponentsInChildren<Transform>();
        */
    }

    void Update()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white; // Устанавливаем цвет
        Vector3 offset = new Vector3(0, 5, 0);

        // Рисуем точки (сферы)
        foreach (var waypoint in tileMap)
        {
            Gizmos.DrawSphere(waypoint.position + offset, 0.3f);
        }

        // Рисуем линии между точками
        for (int i = 0; i < tileMap.Length - 1; i++)
        {
            Gizmos.DrawLine(tileMap[i].position + offset, tileMap[i + 1].position + offset);
        }
    }
}
