using UnityEngine;
using Unity.Netcode;

public class PlayerTokenDebug : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        Debug.Log(
            $"Player spawned | ClientId: {OwnerClientId} | " +
            $"IsOwner: {IsOwner} | IsServer: {IsServer}"
        );

        // Только сервер задаёт начальную позицию.
        if (IsServer)
        {
            transform.position = new Vector3(
                OwnerClientId * 2f,
                0.5f,
                0f
            );
        }
    }
}