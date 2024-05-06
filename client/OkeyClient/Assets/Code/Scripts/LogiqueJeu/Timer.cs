using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // make this class an instance
    public static Timer Instance;

    [SerializeField]
    private TextMeshProUGUI TimerTxt;

    [SerializeField]
    public static float TimerValue = 59;
    private float RemainingTime = TimerValue;
    private bool TimerOn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // this.LaunchTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn)
        {
            if (RemainingTime > 0)
            {
                RemainingTime -= Time.deltaTime;
            }
            else
            {
                TimerOn = false;
                RemainingTime = 0;
            }
            int min = Mathf.FloorToInt(RemainingTime / 60);
            int sec = Mathf.FloorToInt(RemainingTime % 60);
            TimerTxt.text = string.Format("{0:00}:{1:00}", min, sec);
        }
    }

    public float GetRemainingTime()
    {
        return RemainingTime;
    }

    public void LaunchTimer()
    {
        this.TimerOn = true;
        // reset the timer t 59 seconds
        RemainingTime = TimerValue;
    }
}
