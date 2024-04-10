using System;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public class SignalRConnector : MonoBehaviour
{
    private HubConnection hubConnection;

    private async void Start()
    {
        hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:5105/hub").Build();

        // Setup event listeners, if any
        ConfigureHubEvents();

        // Start the connection
        try
        {
            await hubConnection.StartAsync();
            Debug.Log("SignalR connection started.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"SignalR connection failed: {ex.Message}");
        }
    }

    private void ConfigureHubEvents()
    {
        // Example: Listen for a message from the server
        hubConnection.On<string>(
            "ReceiveMessage",
            (message) =>
            {
                Debug.Log($"Message received from server: {message}");
            }
        );
    }

    private async void OnDestroy()
    {
        // Stop the connection when the GameObject is destroyed
        if (hubConnection != null)
        {
            await hubConnection.StopAsync();
            await hubConnection.DisposeAsync();
        }
    }
}
