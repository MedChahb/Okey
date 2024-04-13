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
        this.hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:3030/OkeyHub")
            .Build(); // Update this URL accordingly

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
                Debug.Log("Affichage des rooms");
                //Afficher les rooms
                foreach (var room in rooms.listRooms)
                {
                    if (room == null)
                    {
                        Debug.Log("Null");
                    }
                    Debug.Log($"Nom de room: {room.Name}");
                    Debug.Log($"Nom de room: {room.Capacity}");
                    Debug.Log($"Nom de room {room.Players.Count}");

                    if (rooms.listRooms.Count > 0)
                    {
                        LobbyManager.Instance.rommsListFilled = true;
                        Debug.Log("list is filled");
                    }
                }
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

    //private void ConfigureHubEvents()
    //{
    //    Debug.Log("Configuring hub events.");
    //    this.hubConnection.On<RoomsPacket>("UpdateRoomsRequest", (rooms) =>
    //    {
    //        Debug.Log("Received rooms data, updating LobbyManager.");

    //        // Check if the LobbyManager instance is ready
    //        if (LobbyManager.Instance != null)
    //        {
    //            Debug.Log("Updating rooms in LobbyManager.");

    //            // Directly updating rooms in LobbyManager
    //            LobbyManager.Instance.rooms = rooms;
    //            Debug.Log("Rooms updated successfully with " + rooms.listRooms.Count + " rooms.");

    //            // Optionally log details of received rooms
    //            foreach (var room in rooms.listRooms)
    //            {
    //                Debug.Log($"Room received: {room.Name}, Capacity: {room.Capacity}, Players: {string.Join(", ", room.Players)}");
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogError("LobbyManager instance is null when trying to update rooms.");
    //        }
    //    });

    //    this.hubConnection.On<PacketSignal>("ReceiveMessage", message =>
    //    {
    //        Debug.Log("Received message: " + message._message);
    //    });
    //}



    //public void updateRooms(RoomsPacket newRooms)
    //{
    //    rooms = newRooms;
    //    if (rooms.listRooms.Count > 0)
    //    {
    //        Debug.Log("Rooms updated successfully with " + rooms.listRooms.Count + " rooms.");
    //    }
    //    else
    //    {
    //        Debug.LogError("Rooms data is not initialized properly or the list is empty.");
    //    }
    //}




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
