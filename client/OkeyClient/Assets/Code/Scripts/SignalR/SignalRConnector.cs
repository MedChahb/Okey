using System;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public class SignalRConnector : MonoBehaviour
{
    private HubConnection hubConnection;

    // This method should be called at the start of a game, initialize lobby ~ not sure when
    private async void Start()
    {
        // TODO: the url server isnt right check with backend
        hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:5105/hub").Build();

        // Setup event listeners, if any messages are expected from the server that is
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
        // TODO: Listen for a message from the server to test connection
        hubConnection.On<string>(
            "ReceiveMessage",
            (message) =>
            {
                Debug.Log($"Message received from server: {message}");
            }
        );

        // TODO:
    }

    private async void OnDestroy()
    {
        // Stop the connection when the GameObject is destroyed (Terminate)
        if (hubConnection != null)
        {
            await hubConnection.StopAsync();
            await hubConnection.DisposeAsync();
        }
    }

    // Send Board state as a message to the server
    // TODO: Test this method
    public async void SendBoardState(TuileData[,] boardState)
    {
        try
        {
            await hubConnection.InvokeAsync("SendBoardState", boardState); // for wait for confirmation ~
            Debug.Log("Board state sent successfully."); // help notice
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send board state: {ex.Message}"); // help notice
        }
    }
}
