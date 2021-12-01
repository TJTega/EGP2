using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    /// <summary>
    /// This holds a reference to the UI Manager
    /// </summary>
    public UIManager manager;
    /// <summary>
    /// This holds a reference to a rider controller
    /// </summary>
    public RiderController rc;

    /// <summary>
    /// This is a slider to set the effort manually
    /// </summary>
    [Range(0, 3)]
    public int effort;
    /// <summary>
    /// This is the timer length
    /// </summary>
    public int timerLength;
    /// <summary>
    /// This is the string format output of the timer
    /// </summary>
    public string timerOutput;

    private void Update()
    {
        //Displays effort on a specific ridercontroller
        manager.DisplayEffort(rc, effort);
        //if timer is set run code
        if (manager.timer != null)
        {
            //updates session slider
            manager.UpdateSessionSlider();
            //updates timer
            timerOutput = manager.timer.GetTimerDisplay();
        }
    }

    /// <summary>
    /// Starts timer through UI Manager from debug script
    /// </summary>
    [ContextMenu("Start Timer")]
    private void StartTimer()
    {
        manager.StartTime(timerLength);
    }
}
