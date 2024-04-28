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

    public void SetMainPlayerTurnSignal(bool value)
    {
        mainPlayerTurnSignal.gameObject.SetActive(value);
        player2TurnSignal.gameObject.SetActive(!value);
        player3TurnSignal.gameObject.SetActive(!value);
        player4TurnSignal.gameObject.SetActive(!value);
    }
}
