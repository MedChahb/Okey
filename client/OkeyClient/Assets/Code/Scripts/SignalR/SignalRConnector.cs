using System;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public class SignalRConnector : MonoBehaviour
{
    private HubConnection hubConnection;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // This method should be called at the start of a game, initialize lobby ~ not sure when
    public async void InitializeConnection()
    {
        // TODO: the url server isnt right check with backend
        hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:5105/hub").Build(); // to be changed

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
        // GameObject is destroyed ? terminate
        if (hubConnection != null && hubConnection.State == HubConnectionState.Connected)
        {
            await hubConnection.StopAsync();
            await hubConnection.DisposeAsync();
        }
    }

    // Send Board state as a message to the server
    // TODO: Test this method
    //public async void SendBoardState(TuileData[,] boardState)
    //{
    //    try
    //    {
    //        await hubConnection.InvokeAsync("SendBoardState", boardState); // for wait for confirmation ~
    //        Debug.Log("Board state sent successfully."); // help notice
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError($"Failed to send board state: {ex.Message}"); // help notice
    //    }
    //}

    public async void SendBoardState(TuileData[,] boardState)
    {
        // Ensure the connection is open before attempting to send a message
        if (hubConnection != null && hubConnection.State == HubConnectionState.Connected)
        {
            try
            {
                await hubConnection.InvokeAsync("SendBoardState", boardState);
                Debug.Log("Board state sent successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to send board state: {ex.Message}");
            }
        }
        else
        {
            Debug.LogError("Hub connection is not established.");
        }
    }
}
