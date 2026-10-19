using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Общие данные")]
    [SerializeField] private string nickname;
    public Role role;
    public int hp;
    public int maxHp;

    [Header("Управление")]
    public InputActionReference rollForTurn;

    [Header("Другие скрипты")]
    private BoardMovement boardMovement;

    public enum Role
    {
        Warrior,
        Mage,
        Rouge,
        Cleric
    }

    void Start()
    {
        boardMovement = GetComponent<BoardMovement>();
    }

    void OnEnable()
    {
        rollForTurn.action.started += RollForTurn;
    }

    void Update()
    {
        
    }

    void RollForTurn(InputAction.CallbackContext context)
    {
        int roll = UnityEngine.Random.Range(1, 11);
        Debug.Log("Результат броска: " + roll);
        boardMovement.MoveOnBoard(roll);
    }

    /*
    void MoveOnBoard(int move)
    {
        Debug.Log("Двигаюсь на " + move);
        Transform[] waypoints = tm.tileMap;
        Vector3 offset = new Vector3(0, 12, 0);
        for (int i = 0; i < move - 1; i++)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[i].position + offset, speed * Time.deltaTime);
        }
        isMoving = false;
    }
    */
}
