using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    [SerializeField]
    private TextMeshProUGUI TimerTxt;

    [SerializeField]
    private float TimerValue = 40;
    private float RemainingTime;
    private bool TimerOn;

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

    void Start()
    {
        // LaunchTimer();
    }

    void Update()
    {
        if (TimerOn)
        {
            if (RemainingTime > 0)
            {
                RemainingTime -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                ResetAndRestartTimer();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        int min = Mathf.FloorToInt(RemainingTime / 60);
        int sec = Mathf.FloorToInt(RemainingTime % 60);
        TimerTxt.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    public void LaunchTimer()
    {
        this.TimerOn = true;
        this.RemainingTime = TimerValue;
    }

    public void StopTimer()
    {
        this.TimerOn = false;
    }

    private void ResetAndRestartTimer()
    {
        RemainingTime = TimerValue;
        TimerOn = true;
        UpdateTimerDisplay();
    }
}
