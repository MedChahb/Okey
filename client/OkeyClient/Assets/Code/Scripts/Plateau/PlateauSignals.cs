using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateauSignals : MonoBehaviour
{
    public static PlateauSignals Instance;

    public Image mainPlayerTurnSignal;
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

        if (mainPlayerTurnSignal != null)
        {
            mainPlayerTurnSignal.gameObject.SetActive(true);
            Debug.Log("Main player turn signal set to active.");
        }
        else
        {
            Debug.LogError("Main player turn signal is null.");
        }

        if (player2TurnSignal != null)
            player2TurnSignal.gameObject.SetActive(false);
        else
            Debug.LogError("Player 2 turn signal is null.");

        if (player3TurnSignal != null)
            player3TurnSignal.gameObject.SetActive(false);
        else
            Debug.LogError("Player 3 turn signal is null.");

        if (player4TurnSignal != null)
            player4TurnSignal.gameObject.SetActive(false);
        else
            Debug.LogError("Player 4 turn signal is null.");

        Debug.Log("Main player turn signal setup complete.");
    }

    public void SetPlayerSignal(string playerName)
    {
        Debug.Log("SetPlayerSignal called.");
        // MainThreadDispatcher.Enqueue(() =>
        // {
        // Debug.Log($"Setting turn signal for player: {playerName}");

        mainPlayerTurnSignal.gameObject.SetActive(false);
        player2TurnSignal.gameObject.SetActive(false);
        player3TurnSignal.gameObject.SetActive(false);
        player4TurnSignal.gameObject.SetActive(false);

        //  Debug.Log("player 2:" + LobbyManager.Instance.player2);
        //  Debug.Log("player 3:" + LobbyManager.Instance.player3);
        //  Debug.Log("player 4:" + LobbyManager.Instance.player4);

        playerName = playerName.Trim().ToLower();
        if (LobbyManager.Instance.player2 == playerName)
        {
            player2TurnSignal.gameObject.SetActive(true);
            Debug.Log("Player 2 turn signal set.");
        }
        else if (LobbyManager.Instance.player3 == playerName)
        {
            player3TurnSignal.gameObject.SetActive(true);
            Debug.Log("Player 3 turn signal set.");
        }
        else if (LobbyManager.Instance.player4 == playerName)
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
