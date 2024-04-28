using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateauSignals : MonoBehaviour
{
    public static PlateauSignals Instance;

    [SerializeField]
    private Image mainPlayerTurnSignal;

    [SerializeField]
    private Image player2TurnSignal;

    [SerializeField]
    private Image player3TurnSignal;

    [SerializeField]
    private Image player4TurnSignal;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetMainPlayerTurnSignal()
    {
        mainPlayerTurnSignal.gameObject.SetActive(LobbyManager.Instance.myTurn);
        player2TurnSignal.gameObject.SetActive(false);
        player3TurnSignal.gameObject.SetActive(false);
        player4TurnSignal.gameObject.SetActive(false);
    }

    public void SetPlayerSignal(string playerName)
    {
        if (LobbyManager.Instance.players.Count > 2)
        {
            player2TurnSignal.gameObject.SetActive(false);
            player3TurnSignal.gameObject.SetActive(false);
            player4TurnSignal.gameObject.SetActive(false);

            if (playerName == LobbyManager.Instance.players[0])
            {
                player2TurnSignal.gameObject.SetActive(true);
            }
            else if (playerName == LobbyManager.Instance.players[1])
            {
                player3TurnSignal.gameObject.SetActive(true);
            }
            else if (playerName == LobbyManager.Instance.players[2])
            {
                player4TurnSignal.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Not enough players to set signals.");
        }
    }
}
