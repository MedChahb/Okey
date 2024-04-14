using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEditor;
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
        this.hubConnection = new HubConnectionBuilder().WithUrl(Constants.SIGNALR_HUB_URL).Build(); // Update this URL accordingly

        this.ConfigureHubEvents();

        try
        {
            await this.hubConnection.StartAsync();
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
        this.hubConnection.On<RoomsPacket>(
            "UpdateRoomsRequest",
            (rooms) =>
            {
                Debug.Log("Received room update request.");
                foreach (var room in rooms.listRooms)
                {
                    if (room == null)
                    {
                        Debug.Log("Null");
                        continue;
                    }
                    Debug.Log($"Nom de room: {room.Name}");
                    Debug.Log($"Nom de room: {room.Capacity}");
                    Debug.Log($"Nom de room {room.Players.Count}");
                }

                if (rooms.listRooms.Count > 0)
                {
                    LobbyManager.Instance.rommsListFilled = true;
                    Debug.Log("list is filled");
                    Debug.Log("rooms count ----> " + rooms.listRooms.Count);
                }
                UpdateRoomDisplay(rooms);
            }
        );

        this.hubConnection.On<PacketSignal>(
            "ReceiveMessage",
            message =>
            {
                Debug.Log(message._message);
            }
        );
    }

    public void UpdateRoomDisplay(RoomsPacket roomsPacket)
    {
        if (DisplayRooms.Instance != null)
        {
            DisplayRooms.Instance.SetRooms(roomsPacket);
        }
        else
        {
            Debug.LogError("DisplayRooms instance is not found.");
        }
    }

    public async Task JoinRoom() // takes parameter roomName
    {
        //if (this.hubConnection != null && this.hubConnection.State == HubConnectionState.Connected)
        //{
        //    try
        //    {
        //        await this.hubConnection.SendAsync("JoinRoom", roomName);
        //        Debug.Log($"Request to join room '{roomName}' sent.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogError($"Failed to join room '{roomName}': {ex.Message}");
        //    }
        //}
        //else
        //{
        //    Debug.LogError("Cannot join room. Hub connection is not established.");
        //}


        if (this.hubConnection != null && this.hubConnection.State == HubConnectionState.Connected)
        {
            try
            {
                await hubConnection.SendAsync("LobbyConnection", "room1");
                Debug.Log($"Request to join room 1 sent.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to join room 1: {ex.Message}");
            }
        }
        else
        {
            Debug.LogError("Cannot join room. Hub connection is not established.");
        }
    }

    private async void OnDestroy()
    {
        if (this.hubConnection != null && this.hubConnection.State == HubConnectionState.Connected)
        {
            await this.hubConnection.StopAsync();
            await this.hubConnection.DisposeAsync();
        }
    }

    public async void SendBoardState(TuileData[,] boardState)
    {
        // Ensure the connection is open before attempting to send a message
        if (this.hubConnection != null && this.hubConnection.State == HubConnectionState.Connected)
        {
            try
            {
                await this.hubConnection.InvokeAsync("SendBoardState", boardState);
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
