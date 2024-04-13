using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public class PacketSignal
{
    public string _message { get; set; }
}

public class RoomDto
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public List<string> Players { get; set; }
}

public class RoomsPacket
{
    public List<RoomDto> listRooms { get; set; }
}

public class SignalRConnector : MonoBehaviour
{
    private HubConnection hubConnection;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public async void InitializeConnection()
    {
        hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:3030/OkeyHub").Build(); // Update this URL accordingly

        ConfigureHubEvents();

        try
        {
            await hubConnection.StartAsync();
            Debug.Log("SignalR connection established.");
            LobbyManager.Instance.SetConnectionStatus(true);
        }
        catch (Exception ex)
        {
            Debug.LogError($"SignalR connection failed: {ex.Message}");
        }
    }

    private void ConfigureHubEvents()
    {
        hubConnection.On<RoomsPacket>(
            "UpdateRoomList",
            roomsPacket =>
            {
                Debug.Log("Received room list update.");
                foreach (var room in roomsPacket.listRooms)
                {
                    Debug.Log(
                        $"Room: {room.Name}, Capacity: {room.Capacity}, Players: {string.Join(", ", room.Players)}"
                    );
                }
            }
        );

        hubConnection.On<PacketSignal>(
            "ReceiveMessage",
            message =>
            {
                Debug.Log(message._message);
            }
        );
    }

    public async Task JoinRoom(string roomName)
    {
        if (hubConnection != null && hubConnection.State == HubConnectionState.Connected)
        {
            try
            {
                await hubConnection.SendAsync("JoinRoom", roomName);
                Debug.Log($"Request to join room '{roomName}' sent.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to join room '{roomName}': {ex.Message}");
            }
        }
        else
        {
            Debug.LogError("Cannot join room. Hub connection is not established.");
        }
    }

    private async void OnDestroy()
    {
        if (hubConnection != null && hubConnection.State == HubConnectionState.Connected)
        {
            await hubConnection.StopAsync();
            await hubConnection.DisposeAsync();
        }
    }

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
