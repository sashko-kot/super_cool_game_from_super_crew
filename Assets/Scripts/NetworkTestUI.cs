using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class NetworkTestUI : MonoBehaviour
{
    [Header("UI")]
    //[SerializeField] private Button hostButton;
    //[SerializeField] private Button clientButton;
    [SerializeField] private TMP_Text statusText;

    [Header("Connection")]
    [SerializeField] private string hostAddress = "127.0.0.1";
    [SerializeField] private ushort port = 7777;

    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.Singleton;

        if (networkManager == null)
        {
            SetStatus("ERROR: NetworkManager not found");
            return;
        }

        networkManager.OnClientConnectedCallback += OnClientConnected;
        networkManager.OnClientDisconnectCallback += OnClientDisconnected;

        SetStatus("Choose Host or Client");
    }

    private void OnDestroy()
    {
        if (networkManager == null)
            return;

        networkManager.OnClientConnectedCallback -= OnClientConnected;
        networkManager.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    public void StartHost()
    {
        if (!CanStart())
            return;

        bool started = networkManager.StartHost();

        SetStatus(started
            ? "Host started. Waiting for clients..."
            : "ERROR: Could not start Host");
    }

    public void StartClient()
    {
        if (!CanStart())
            return;

        UnityTransport transport =
            networkManager.GetComponent<UnityTransport>();

        if (transport == null)
        {
            SetStatus("ERROR: UnityTransport not found");
            return;
        }

        transport.SetConnectionData(hostAddress, port);

        bool started = networkManager.StartClient();

        SetStatus(started
            ? "Client started. Connecting..."
            : "ERROR: Could not start Client");
    }

    public void DisconnectClient()
    {
        if (networkManager == null)
        {
            SetStatus("ERROR: NetworkManager not found");
            return;
        }

        if (networkManager.IsServer)
        {
            SetStatus("This instance is Host/Server. Not disconnecting.");
            return;
        }

        if (!networkManager.IsClient || !networkManager.IsListening)
        {
            SetStatus("Client is not connected");
            return;
        }

        networkManager.Shutdown();
        SetStatus("Client disconnected locally");
    }

    private bool CanStart()
    {
        if (networkManager == null)
        {
            SetStatus("ERROR: NetworkManager not found");
            return false;
        }

        if (networkManager.IsListening)
        {
            SetStatus("Network already started");
            return false;
        }

        return true;
    }

    private void OnClientConnected(ulong clientId)
    {
        SetStatus(
            $"Connected: {clientId} | " +
            $"Players: {networkManager.ConnectedClientsIds.Count}"
        );
    }

    private void OnClientDisconnected(ulong clientId)
    {
        SetStatus($"Disconnected: {clientId}");
    }

    private void SetStatus(string message)
    {
        Debug.Log("[NetworkTest] " + message);

        if (statusText != null)
            statusText.text = message;
    }
}