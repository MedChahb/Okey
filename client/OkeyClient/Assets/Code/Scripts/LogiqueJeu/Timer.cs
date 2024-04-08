using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI TimerTxt;
    
    [SerializeField] public static float TimerValue=30;
    private float RemainingTime=TimerValue; // timer value
    private bool TimerOn;
    
    // Start is called before the first frame update
    void Start()
    {
        this.LaunchTimer(); // pour test apres on verra ou il faut l'appeler 
    }

    // Update is called once per frame
    void Update()
    {
        if(TimerOn){
            if(RemainingTime>0){
            RemainingTime-=Time.deltaTime; 
        }else{
            RemainingTime=0;
            TimerOn=false;
            RemainingTime=TimerValue; //reinit timer
        }

        int min=Mathf.FloorToInt(RemainingTime/60);
        int sec=Mathf.FloorToInt(RemainingTime%60);
        TimerTxt.text=string.Format("{0:00}:{1:00}",min,sec);
        }
        
    }


    public float GetRemainingTime(){return RemainingTime;}

    public void LaunchTimer(){
        this.TimerOn=true;
        }

}
