using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateauSignals : MonoBehaviour
{
    public static PlateauSignals Instance;

    public Image TuileCentre;
    public Image TuileGauche;
    public Image player2TurnSignal;
    public Image player3TurnSignal;
    public Image player4TurnSignal;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetMainPlayerTurnSignal()
    {
        Debug.Log("Setting main player turn signal.");

        TuileCentre.gameObject.SetActive(true);
        Debug.Log("Main player turn signal set to active.");
        player2TurnSignal.gameObject.SetActive(false);
        player3TurnSignal.gameObject.SetActive(false);
        player4TurnSignal.gameObject.SetActive(false);

        Debug.Log("Main player turn signal setup complete.");
    }

    public void SetPlayerSignal(string playerName)
    {
        Debug.Log("SetPlayerSignal called.");
        // MainThreadDispatcher.Enqueue(() =>
        // {
        // Debug.Log($"Setting turn signal for player: {playerName}");

        TuileCentre.gameObject.SetActive(false);
        player2TurnSignal.gameObject.SetActive(false);
        player3TurnSignal.gameObject.SetActive(false);
        player4TurnSignal.gameObject.SetActive(false);

        //  Debug.Log("player 2:" + LobbyManager.Instance.player2);
        //  Debug.Log("player 3:" + LobbyManager.Instance.player3);
        //  Debug.Log("player 4:" + LobbyManager.Instance.player4);

        playerName = playerName.Trim().ToLower();
        if (LobbyManager.player2 == playerName)
        {
            player2TurnSignal.gameObject.SetActive(true);
            Debug.Log("Player 2 turn signal set.");
        }
        else if (LobbyManager.player3 == playerName)
        {
            player3TurnSignal.gameObject.SetActive(true);
            Debug.Log("Player 3 turn signal set.");
        }
        else if (LobbyManager.player4 == playerName)
        {
            player4TurnSignal.gameObject.SetActive(true);
            Debug.Log("Player 4 turn signal set.");
        }
        else
        {
            Debug.LogError($"Player name {playerName} does not match any known player.");
        }
        // });
    }
}
