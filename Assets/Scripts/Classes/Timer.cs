using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    /// <summary>
    /// This holds the start time for the timer in minutes
    /// </summary>
    private float startTime;
    /// <summary>
    /// This holds the time remaining in seconds
    /// </summary>
    private int timeRemaining;
    /// <summary>
    /// This holds the time remaining in the timer in 00:00:00 format
    /// </summary>
    private string ouput;
    /// <summary>
    /// This is a unity action being called at the timer end
    /// </summary>
    public UnityAction OnTimerEnd;
    /// <summary>
    /// This is used to store the coroutine to make sure it stops
    /// </summary>
    public Coroutine countCoroutine;

    /// <summary>
    /// This sets the amount of time for timer
    /// </summary>
    /// <param name="time">Start time for timer</param>
    public void SetTimer(float time)
    {
        //Sets start time
        startTime = time;
        // Sets remaining time to the start time and converts to seconds
        timeRemaining = Mathf.FloorToInt(startTime * 60);
        //Sets output in 00:00:00 format
        ouput = Mathf.FloorToInt(timeRemaining / 3600).ToString().PadLeft(2, '0') + " : " + Mathf.FloorToInt((timeRemaining / 60) % 60).ToString().PadLeft(2, '0') + " : " + Mathf.FloorToInt(timeRemaining % 60).ToString().PadLeft(2, '0');
    }

    /// <summary>
    /// This starts the timer
    /// </summary>
    [ContextMenu("Start Timer")]
    public void StartTimer()
    {
        countCoroutine = StartCoroutine(Count());
    }

    /// <summary>
    /// This is used to reset the timer if a session is quit mid way through
    /// </summary>
    public void ResetTimer()
    {
        // If there is a corotuine to reset, reset it
        if (countCoroutine != null)
            StopCoroutine(countCoroutine); 
    }

    /// <summary>
    /// This is the function that handles counting and timer display
    /// </summary>
    /// <returns></returns>
    private IEnumerator Count()
    {
        while (timeRemaining >= 0)
        {
            ouput = Mathf.FloorToInt(timeRemaining / 3600).ToString().PadLeft(2, '0') + " : " + Mathf.FloorToInt((timeRemaining / 60) % 60).ToString().PadLeft(2, '0') + " : " + Mathf.FloorToInt(timeRemaining % 60).ToString().PadLeft(2, '0');
            yield return new WaitForSeconds(1);
            timeRemaining--;
        }
        ouput = Mathf.FloorToInt(timeRemaining / 3600).ToString().PadLeft(2, '0') + " : " + Mathf.FloorToInt((timeRemaining / 60) % 60).ToString().PadLeft(2, '0') + " : " + Mathf.FloorToInt(timeRemaining % 60).ToString().PadLeft(2, '0');
        //Call ontimerend
        OnTimerEnd.Invoke();
    }

    /// <summary>
    /// This calls for the time remaining
    /// </summary>
    /// <returns>Remaining time</returns>
    public int GetTimeRemaining()
    {
        return timeRemaining;
    }

    /// <summary>
    /// This stops the timer and resets the timer values
    /// </summary>
    [ContextMenu("Stop Timer")]
    public void StopTimer()
    {
        StopCoroutine(countCoroutine);
        // Resets variables
        startTime = 0;
        timeRemaining = 0;
        ouput = "";
    }

    /// <summary>
    /// This calls for the the timer display
    /// </summary>
    /// <returns>Output string in 00:00:00 format</returns>
    public string GetTimerDisplay()
    {
        return ouput;
    }

}
