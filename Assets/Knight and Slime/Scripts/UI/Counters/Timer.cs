using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TMP_Text text;
    public enum TimerType {Countdown, Stopwatch}
    public TimerType type;

    public float timeToDisplay = 0.0f;

    private bool isRunning;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void OnEnable(){
        EventManager.TimerStart += EventManagerOnTimerStart;
        EventManager.TimerStop += EventManagerOnTimerStop;
        EventManager.TimerUpdate += EventManagerOnTimerUpdate;

    }

    private void OnDisable(){
        EventManager.TimerStart -= EventManagerOnTimerStart;
        EventManager.TimerStop -= EventManagerOnTimerStop;
        EventManager.TimerUpdate -= EventManagerOnTimerUpdate;
    }

    private void EventManagerOnTimerStart(){
        isRunning = true;
    }

    private void EventManagerOnTimerStop(){
        isRunning = false;
    }

    private void EventManagerOnTimerUpdate(float value){
        timeToDisplay += value;
    }

    // Update is called once per frame
    private void Update()
    {
        // if not running do nothing
        if(!isRunning){
            return;
        }

        // if the timer is a countdown and is out of time stop it
        if (type == TimerType.Countdown && timeToDisplay < 0.0f) {
            EventManager.OnTimerStop();
            return;
        }

        // if type is countdown decrease the time if not increase time
        timeToDisplay += (type == TimerType.Countdown) ? -Time.deltaTime : Time.deltaTime;

        // time to be displayed
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeToDisplay);
        // set the text object to the timespan and format it
        text.text = timeSpan.ToString(format:@"mm\:ss\:ff");
    }
}
