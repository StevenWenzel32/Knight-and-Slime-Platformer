using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public static class EventManager
{
    public static event UnityAction TimerStart;
    public static event UnityAction TimerStop;
    public static event UnityAction<float> TimerUpdate;

    // the ? checks if it is null first -- only in C#
    // => no need for curly braces -- only in C#
    public static void OnTimerStart() => TimerStart?.Invoke();
    public static void OnTimerStop() => TimerStop?.Invoke();
    public static void OnTimerUpdate(float value) => TimerUpdate?.Invoke(value);

}
