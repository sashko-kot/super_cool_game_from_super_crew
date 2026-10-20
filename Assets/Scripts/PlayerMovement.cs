using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    private InputSystem_Actions inputActions;
    private Vector3 serverMoveDirection;
    private Vector3 lastSentDirection;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            inputActions = new InputSystem_Actions();
            inputActions.Player.Enable();
        }
    }

    private void Update()
    {
        if (IsServer)
        {
            transform.position +=
                serverMoveDirection * moveSpeed * Time.deltaTime;
        }

        if (!IsOwner || inputActions == null)
            return;

        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 direction = new Vector3(input.x, 0f, input.y);

        if (direction.sqrMagnitude > 1f)
            direction.Normalize();

        if (direction != lastSentDirection)
        {
            lastSentDirection = direction;
            SetMovementServerRpc(direction);
        }
    }

    [Rpc(SendTo.Server)]
    private void SetMovementServerRpc(Vector3 direction)
    {
        if (direction.sqrMagnitude > 1f)
            direction.Normalize();

        serverMoveDirection = direction;
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner && inputActions != null)
        {
            inputActions.Player.Disable();
            inputActions.Dispose();
            inputActions = null;
        }

        if (IsServer)
            serverMoveDirection = Vector3.zero;
    }
}