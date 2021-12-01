using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool calledOnce = false;
    private float resetOffset = 0;
    #region Data
    [Header("Data")]
    /// <summary>
    /// This array holds all of the active bikes
    /// </summary>
    public Bike[] bikes = new Bike[16];
    /// <summary>
    /// This holds a reference to the timer object
    /// </summary>
    public Timer timer;
    public int teamSliderMax = 200;
    /// <summary>
    /// This holds the current session data
    /// </summary>
    private SessionData currentSession;
    /// <summary>
    /// This captures the total generated wh at the start of a session
    /// </summary>
    private float startGeneratedWh;
    /// <summary>
    /// This is the total generated Wh
    /// </summary>
    private int resistanceMode;
    private int resistance;
    private float totalGeneratedWh;
    private bool workoutStarted;
    private bool[] noAdd = new bool[16];
    private float[] teamOutputPower = new float[4];
    #endregion
    #region General Progression
    [Header("General Progression")]
    /// <summary>
    /// This slider controls the big slider for the overall total KWh generated
    /// </summary>
    public Image overallTotalDisplay;
    /// <summary>
    /// This holds a refernce to the current value text
    /// </summary>
    public Text currentOverallTotalValue;
    /// <summary>
    /// This holds the max value for the overall total display slider
    /// </summary>
    public int otdMax;
    /// <summary>
    /// This is the offset value for when power is drained from the battery
    /// </summary>
    public float otdOffset;
    /// <summary>
    /// This slider controls the smaller slider showing the session preview 
    /// </summary>
    public Image sessionPreviewDisplay;
    /// <summary>
    /// This holds the max value for the session preview display slider
    /// </summary>
    [HideInInspector]
    public int spdMax;
    /// <summary>
    /// This slider controls the smaller slider showing the session preview 
    /// </summary>
    public Image individualResistanceDisplay;
    /// <summary>
    /// This holds a reference to the timer text display in individual ride
    /// </summary>
    public Text freeRideTimerText;
    /// <summary>
    /// This holds a reference to the timer text display in individual ride
    /// </summary>
    public Text individualTimerText;
    /// <summary>
    /// This holds a reference to the session phase text in individual ride
    /// </summary>
    public Text individualSessionPhaseText;
    /// <summary>
    /// This slider controls the smaller slider showing the session preview 
    /// </summary>
    public Image headToHeadResistanceDisplay;
    /// <summary>
    /// This is a bool dictating whether a session has started or not
    /// </summary>
    private bool sessionStarted = false;
    /// <summary>
    /// This holds the max value for effort
    /// </summary>
    public int maximumEffort;
    #endregion
    #region References
    [Header("References")]
    ///<summary>
    ///This holds a reference to the data handler
    ///</summary>
    public DataHandler dataHandler;
    ///<summary>
    ///This holds the rider controllers which control the data showed on screen for each free rider
    ///</summary>
    public RiderController[] freeRiders = new RiderController[16];
    ///<summary>
    ///This holds the rider controllers which control the data showed on screen for each head to head rider
    ///</summary>
    public RiderController[] headToHeadRiders = new RiderController[16];
    ///<summary>
    ///This holds the rider controllers which control the data showed on screen for each individual rider
    ///</summary>
    public RiderController[] individualRiders = new RiderController[16];
    /// <summary>
    /// This holds reference to the session data objects
    /// </summary>
    public SessionData[] sessions = new SessionData[20];
    /// <summary>
    /// This holds reference to the summary panels manager
    /// </summary>
    public SummaryPanelsManager summary;
    /// <summary>
    /// This holds reference to the head to head panel manager
    /// </summary>
    public HeadToHeadPanelManager headToHead;
    /// <summary>
    /// This holds a reference to the rank icons, 0 being 1st and 3 being 4th
    /// </summary>
    public Sprite[] rankIcons = new Sprite[4];
    /// <summary>
    /// This holds the current session text that needs to be edited when the session drop down is changed
    /// </summary>
    public Text setupSessionText;
    /// <summary>
    /// This holds the controller for the rider panel's animation on the left side
    /// </summary>
    public AnimatorOverrideController riderPanelControllerLeft;
    /// <summary>
    /// This holds the controller for the rider panel's animation on the right side
    /// </summary>
    public AnimatorOverrideController riderPanelControllerRight;
    /// <summary>
    /// This is used to hold the hide UI button that is currently being used
    /// </summary>
    public GameObject hideUIButton;
    /// <summary>
    /// This is used to hold a reference to the debug settings button
    /// </summary>
    public GameObject debugSettingsButton;
    /// <summary>
    /// This is used to store the session types in an array
    /// </summary>
    public Text[] sessionTypeTexts;
    /// <summary>
    /// This is used to prevent errors with the animator if a position is changed between two bikes rapidly
    /// </summary>
    public List<GameObject> animationIgnoreList = new List<GameObject>();
    /// <summary>
    /// This is used to store the summary buttons and display them when appropriate
    /// </summary>
    public GameObject[] summaryButton;
    /// <summary>
    /// This is used to display how many times the top bar has been filled
    /// </summary>
    public Text totalPowerGeneratedCounter;
    public GameObject[] returnButtons = new GameObject[3];
    public GameObject[] startButtons = new GameObject[3];
    public GameObject[] pauseButtons = new GameObject[3];
    public GameObject[] resumeButtons = new GameObject[3];
    #endregion
    #region Screens
    [Header("Screens")]
    /// <summary>
    /// This holds a reference to the setup screen
    /// </summary>
    public GameObject setupScreen;
    /// <summary>
    /// This holds a reference to the persistent object (logos and big slider)
    /// </summary>
    public GameObject persistentObjects;
    /// <summary>
    /// This holds a reference to the free ride screen
    /// </summary>
    public GameObject freeRideScreen;
    /// <summary>
    /// This holds a reference to the head to head screen
    /// </summary>
    public GameObject headToHeadScreen;
    /// <summary>
    /// This holds a reference to the individual ride screen
    /// </summary>
    public GameObject individualRideScreen;
    /// <summary>
    /// This holds a reference to the edit menu screen
    /// </summary>
    public GameObject editMenuScreen;
    #endregion
    #region Tracking
    [Header("Tracking")]
    /// <summary>
    /// This is used to determin which of the array of session to edit when in the edit menu
    /// </summary>
    public int sessionToEdit;
    /// <summary>
    /// This is used to determine which rider should have their name changed
    /// </summary>
    public int riderNumber;
    /// <summary>
    /// This is used to determine which team should have their name changed
    /// </summary>
    public int teamNumber;
    /// <summary>
    /// This is used to make sure the summary screen only opens automatically if the instructor doesn't
    /// </summary>
    public bool needToOpen = true;
    #endregion
    #region Unity Events
    [Header("Unity Events")]
    public UnityEvent onWorkoutStarted;
    public UnityEvent onWorkoutFinished;
    public UnityEvent onSessionEnd;
    #endregion

    void Start()
    {
        // Set the setup menu's session text
        DisplaySessionTypes();
        // If the playerpref value doesn't exist
        if (!PlayerPrefs.HasKey("TotalPowerGenerated"))
            // Create the playerpref value
            PlayerPrefs.SetFloat("TotalPowerGenerated", 0);
        resistanceMode = 0;
    }

    #region Main Loop

    private void Update()
    {
        UpdateTotalGeneratedSlider();
        // Only run this code if a session has been started
        if (sessionStarted)
        {
            CheckTimes();
            if (timer.GetTimeRemaining() >= 0)
            {
                UpdateSessionSlider();
                freeRideTimerText.text = timer.GetTimerDisplay();
                individualTimerText.text = timer.GetTimerDisplay();
                headToHead.timer.text = timer.GetTimerDisplay();
            }
            switch (currentSession.sessionType)
            {
                // If the session type is freeride, run this code
                case SessionType.freeride:
                    //Displays relevant data for all bikes in array
                    for (int i = 0; i < freeRiders.Length; i++)
                    {
                        RiderController rider = freeRiders[i];
                        Bike bike = rider.bike;

                        rider.riderNameText.text = $"Bike #{bike.bikeNo}";
                        rider.instantPowerText.text = $"{bike.wattsInstant}W";
                        rider.powerOutputText.text = $"{Math.Round(bike.whGenerated - currentSession.summary.sessionWhGenerated[i], 1)}Wh";
                        DisplayEffort(rider, CalculateEffort(bike.wattsInstant));
                    }
                    break;
                case SessionType.head2head:
                    UpdateRanking();
                    UpdateTeamSliders();
                    for (int i = 0; i < headToHeadRiders.Length; i++)
                    {
                        RiderController rider = headToHeadRiders[i];
                        Bike bike = rider.bike;

                        rider.riderNameText.text = bike.riderName;
                        rider.instantPowerText.text = $"{bike.wattsInstant}W";
                        rider.powerOutputText.text = $"{Math.Round(bike.whGenerated - currentSession.summary.sessionWhGenerated[i], 1)}Wh";
                        DisplayEffort(rider, CalculateEffort(bike.wattsInstant));
                    }
                    break;
                case SessionType.individual:
                    UpdateRanking();
                    //Display data to individual rider panels
                    for (int i = 0; i < individualRiders.Length; i++)
                    {
                        RiderController rider = individualRiders[i];
                        Bike bike = rider.bike;
                        //if (!noAdd[i] && workoutStarted)
                        //{
                        //    noAdd[i] = true;
                        //    StartCoroutine(AddToAvg(i, bike));
                        //}
                        rider.riderNameText.text = bike.riderName;
                        rider.instantPowerText.text = $"{bike.wattsInstant}W";
                        rider.powerOutputText.text = $"{Math.Round(bike.whGenerated - currentSession.summary.sessionWhGenerated[i], 1)}Wh";
                        DisplayEffort(rider, CalculateEffort(bike.wattsInstant));
                    }
                    break;
                default:
                    break;
            }
        }
    }
    private IEnumerator AddToAvg(int i, Bike bike)
    {
        yield return new WaitForSeconds(1);
        currentSession.summary.avgOutputPowers[i] += bike.wattsInstant;
        Debug.Log(currentSession.summary.avgOutputPowers[i]);
        noAdd[i] = false;
    }

    #endregion

    #region Update Methods

    /// <summary>
    /// Updates the session preview display
    /// </summary>
    public void UpdateSessionSlider()
    {
        Image currentSlider = null;
        switch (currentSession.sessionType)
        {
            case SessionType.freeride:
                currentSlider = sessionPreviewDisplay;
                break;
            case SessionType.individual:
                currentSlider = individualResistanceDisplay;
                break;
            case SessionType.head2head:
                currentSlider = headToHeadResistanceDisplay;
                break;
        }
        // Sets the value to the max value - the remaining time
        currentSlider.fillAmount = 1.0f - ((float)timer.GetTimeRemaining() / (float)spdMax);
    }

    /// <summary>
    /// This function checks the times to see if certain markers get hit
    /// </summary>
    public void CheckTimes()
    {

        Text sessionPhase = null;
        switch (currentSession.sessionType)
        {
            case SessionType.individual:
                sessionPhase = individualSessionPhaseText;
                break;
            case SessionType.head2head:
                sessionPhase = headToHead.sessionPhase;
                break;
        }

        if (currentSession.sessionType != SessionType.freeride)
        {
            foreach (var resTime in currentSession.resistancePattern)
            {
                if (timer.GetTimeRemaining() == (spdMax / 30) * resTime.Key)
                {
                    resistance = resTime.Value;
                    //Debug.Log(resistance);
                    SerialWrite();
                }
            }
            if (timer.GetTimeRemaining() < (spdMax / 6))
            {
                sessionPhase.text = "COOLDOWN";
                workoutStarted = false;

                //Runs code depending on the session type
                switch (currentSession.sessionType)
                {
                    case SessionType.head2head:
                        // Display the summary button for the free ride session
                        summaryButton[1].gameObject.SetActive(true);
                        break;
                    case SessionType.individual:
                        // Display the summary button for the free ride session
                        summaryButton[2].gameObject.SetActive(true);
                        break;
                }
            }
            else if (timer.GetTimeRemaining() == (spdMax / 6))
            {
                if (!calledOnce)
                {
                    calledOnce = true;
                    Debug.Log("Calling");
                    onWorkoutFinished.Invoke();
                    // If the summary screen hasn't already been opened
                    if (needToOpen)
                        // Open it
                        DisplaySummary(false);
                    workoutStarted = false;
                }
            }
            else if (timer.GetTimeRemaining() < (spdMax / 6) * 5)
            {
                sessionPhase.text = "WORKOUT";
                workoutStarted = true;
            }
            else if (timer.GetTimeRemaining() == (spdMax / 6) * 5)
            {
                onWorkoutStarted.Invoke();
                workoutStarted = true;
            }
            else if (timer.GetTimeRemaining() <= spdMax)
            {
                sessionPhase.text = "WARMUP";
                workoutStarted = false;
            }
        }
        else
        {
            resistance = currentSession.resistanceValue;
            //Debug.Log(resistance);
            SerialWrite();
        }
        if (timer.GetTimeRemaining() <= (spdMax / 6))
        {
            if (currentSession.sessionType == SessionType.freeride)
            {
                summaryButton[0].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// This updates the total KWh generated slider at the top
    /// </summary>
    public void UpdateTotalGeneratedSlider()
    {
        // Accumulates all generated Wh from all bikes
        totalGeneratedWh = 0;
        foreach (Bike bike in bikes)
        {
            totalGeneratedWh += bike.whGenerated;
        }
        //Sets value to total generated across all bike minus offset
        overallTotalDisplay.fillAmount = ((totalGeneratedWh + PlayerPrefs.GetFloat("TotalPowerGenerated") + otdOffset) % otdMax) / otdMax;
        //PlayerPrefs.SetFloat("TotalPowerGenerated", (float)Math.Round((totalGeneratedWh + resetOffset + otdOffset) / 1000, 1));
        //Displays current value on slider in numeric form at one decimal place
        currentOverallTotalValue.text = $"{Math.Round(((totalGeneratedWh + PlayerPrefs.GetFloat("TotalPowerGenerated") + otdOffset) % otdMax)/1000, 2)} kWh";
        // Update the text to display the counter
        totalPowerGeneratedCounter.text = $"x{Mathf.FloorToInt((totalGeneratedWh + PlayerPrefs.GetFloat("TotalPowerGenerated") + otdOffset) / otdMax)}";
    }

    /// <summary>
    /// This updates the rider positions on screen for individual ride and the rank icons for head to head
    /// </summary>
    private void UpdateRanking()
    {
        switch (currentSession.sessionType)
        {
            case SessionType.individual:
                Bike tempBike;
                string tempName;
                float tempSessionWh;
                //float tempAvgPower;
                for (int i = individualRiders.Length - 1; i > 0; i--)
                {
                    if (workoutStarted && (individualRiders[i].bike.whGenerated - currentSession.summary.sessionWhGenerated[i]) > (individualRiders[i - 1].bike.whGenerated - currentSession.summary.sessionWhGenerated[i - 1]))
                    {
                        // Need to make sure transitions only happen to 0-7 ranks, with 8-16 updating rank without changing position. Only 8 on the contender's side can transition when it replaces 7
                        // May need to place this VVV bit into the animation function and have it trigger after the corotuine has animated the rider panel's off screen

                        tempBike = individualRiders[i].bike;
                        tempName = currentSession.riderNames[i];
                        tempSessionWh = currentSession.summary.sessionWhGenerated[i];
                        //tempAvgPower = currentSession.summary.avgOutputPowers[i];

                        // Commented this out since it wasn't working as intended - Look into again to see if there's a means to handle this better
                        // If neither of the bikes are currently already transitioning - Use to stop rapid back and forth transitions
                        if (!animationIgnoreList.Contains(individualRiders[i].gameObject))
                        {
                            // Play animation for both riders
                            TriggerAnimation(individualRiders[i].gameObject);
                        }
                        // If neither of the bikes are currently already transitioning - Use to stop rapid back and forth transitions
                        if (!animationIgnoreList.Contains(individualRiders[i - 1].gameObject))
                        {
                            // Play animation for both riders
                            TriggerAnimation(individualRiders[i - 1].gameObject);
                        }

                        individualRiders[i].bike = individualRiders[i - 1].bike;
                        currentSession.riderNames[i] = currentSession.riderNames[i - 1];
                        currentSession.summary.sessionWhGenerated[i] = currentSession.summary.sessionWhGenerated[i - 1];

                        individualRiders[i - 1].bike = tempBike;
                        currentSession.riderNames[i - 1] = tempName;
                        currentSession.summary.sessionWhGenerated[i - 1] = tempSessionWh;

                        //individualRiders[i].bike = individualRiders[i - 1].bike;
                        //currentSession.riderNames[i] = currentSession.riderNames[i - 1];
                        //currentSession.summary.sessionWhGenerated[i] = currentSession.summary.sessionWhGenerated[i - 1];
                        ////currentSession.summary.avgOutputPowers[i] = currentSession.summary.avgOutputPowers[i - 1];

                        //individualRiders[i - 1].bike = tempBike;
                        //currentSession.riderNames[i - 1] = tempName;
                        //currentSession.summary.sessionWhGenerated[i - 1] = tempSessionWh;


                        // Update the rank info at a timed delay
                        //StartCoroutine(DelayedRankUpdate(i, individualRiders[i].bike, currentSession.riderNames[i], currentSession.summary.sessionWhGenerated[i]));



                    }
                }
                
                break;
            case SessionType.head2head:
                float tempRank;
                float[] temp = CopyFloatArray(teamOutputPower);
                foreach (var outputPower in teamOutputPower)
                {
                    for (int i = teamOutputPower.Length - 1; i > 0; i--)
                    {
                        if (temp[i] > temp[i - 1])
                        {
                            tempRank = temp[i];
                            temp[i] = temp[i - 1];
                            temp[i - 1] = tempRank;
                        }
                    }
                }
                //Debug.Log($"ORDERED LIST: {temp[0]}, {temp[1]}, {temp[2]}, {temp[3]}");
                for (int i = 0; i < headToHead.positions.Length; i++)
                {
                    headToHead.positions[i].sprite = rankIcons[Array.IndexOf(temp, teamOutputPower[i])];
                }
                //Debug.Log($"RAW LIST: {teamOutputPower[0]}, {teamOutputPower[1]}, {teamOutputPower[2]}, {teamOutputPower[3]}");

                break;
        }
    }

    /// <summary>
    /// This copies float arrays
    /// </summary>
    /// <param name="current">The array intended to copy</param>
    /// <returns>The copied array</returns>
    float[] CopyFloatArray(float[] current)
    {
        float[] temp = new float[current.Length];
        for (int i = 0; i < current.Length; i++)
        {
            temp[i] = current[i];
        }
        return temp;
    }

    private void UpdateTeamSliders()
    {
        for (int i = 0; i < headToHead.teamPowerDisplays.Length; i++)
        {
            Image currentSlider = headToHead.teamPowerDisplays[i];
            // Set the current slider's anchor
            currentSlider.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            currentSlider.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            currentSlider.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            // Set the native size of the slider
            currentSlider.SetNativeSize();

            teamOutputPower[i] = 0;
            int ri = i * 4;
            int rim = (i + 1) * 4;
            for (int rii = ri; rii < rim; rii++)
            {
                // If the loop count isn't above the size of the arrays used
                if (rii <= currentSession.summary.sessionWhGenerated.Length && rii <= headToHeadRiders.Length && i <= teamOutputPower.Length)
                    teamOutputPower[i] += headToHeadRiders[rii].bike.whGenerated - currentSession.summary.sessionWhGenerated[rii];
            }
            currentSlider.fillAmount = teamOutputPower[i] / teamSliderMax;
            headToHead.teamPowerQuantityDisplay[i].text = $"{Math.Round(teamOutputPower[i], 2)}Wh";
        }
    }

    #endregion

    #region Pause Methods

    public void PauseSession()
    {
        Time.timeScale = 0;
    }

    public void UnpauseSession()
    {
        Time.timeScale = 1;
    }

    #endregion

    #region Effort Levels

    /// <summary>
    /// This converts the current power into an effort level
    /// </summary>
    /// <param name="currentPower">The current ouput power of a bike</param>
    /// <returns></returns>
    public int CalculateEffort(int currentPower)
    {
        if (currentPower > (maximumEffort / 3) * 2)
        {
            return 3;
        }
        else if (currentPower > maximumEffort / 3)
        {
            return 2;
        }
        else if (currentPower > 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// This function updates the effort level display
    /// </summary>
    /// <param name="rider">This is the rider controller affected</param>
    /// <param name="effortLevel">This is the level of effort from the rider based on current power</param>
    public void DisplayEffort(RiderController rider, int effortLevel)
    {
        // Sets all effort bars to not active
        foreach (var bar in rider.effortMeterBars)
        {
            bar.enabled = false;
        }
        // For each effort level add a bar
        for (int i = 0; i < effortLevel; i++)
        {
            rider.effortMeterBars[i].enabled = true;
        }
    }

    #endregion

    #region Start Functions

    /// <summary>
    /// This function sets the timer, sets the max value of the session preview slider and starts the timer
    /// </summary>
    /// <param name="length">This is the value to count down from</param>
    public void StartTime(float length = 0)
    {
        if (length == 0)
        {
            length = currentSession.timerLength;
        }
        //Sets timer length
        timer.SetTimer(length);
        //Sets max slider value
        spdMax = Mathf.FloorToInt(length * 60);
        //Starts timer
        timer.StartTimer();
    }

    /// <summary>
    /// This function starts the session
    /// </summary>
    /// <param name="sessionID">The index of the session in the sessions array</param>
    public void StartSession(int sessionID)
    {
        //Sets the OnTimerEnd function
        timer.OnTimerEnd = () => onSessionEnd.Invoke();
        //Sets relevant screens
        setupScreen.SetActive(false);
        persistentObjects.SetActive(true);
        //Sets current session
        currentSession = sessions[sessionID];
        //Does actions based on the session type
        //startGeneratedWh = totalGeneratedWh;
        switch (currentSession.sessionType)
        {
            case SessionType.freeride:
                freeRideScreen.SetActive(true);
                for (int i = 0; i < freeRiders.Length; i++)
                {
                    freeRiders[i].bike = bikes[i];
                }
                break;
            case SessionType.head2head:
                headToHeadScreen.SetActive(true);

                for (int i = 0; i < currentSession.teamNames.Length; i++)
                {
                    headToHead.teamNames[i].text = currentSession.teamNames[i];
                    currentSession.summary.teamRanks[i] = i + 1;
                }

                for (int i = 0; i < headToHeadRiders.Length; i++)
                {
                    headToHeadRiders[i].bike = bikes[i];
                    headToHeadRiders[i].bike.riderName = currentSession.riderNames[i];
                }

                break;
            case SessionType.individual:
                individualRideScreen.SetActive(true);
                for (int i = 0; i < individualRiders.Length; i++)
                {
                    individualRiders[i].bike = bikes[i];
                    individualRiders[i].rank = i + 1;
                    individualRiders[i].bike.riderName = currentSession.riderNames[i];
                    //currentSession.summary.avgOutputPowers[i] = 0;
                }
                break;
        }
        for (int i = 0; i < startButtons.Length; i++)
        {
            returnButtons[i].SetActive(true);
            startButtons[i].SetActive(true);
            pauseButtons[i].SetActive(false);
            resumeButtons[i].SetActive(false);
        }
        totalGeneratedWh = 0;
        timer.SetTimer(currentSession.timerLength);
        calledOnce = false;
        //Starts session
        sessionStarted = true;
    }

    #endregion

    #region Workout Events
    /// <summary>
    /// Sets relevant workout offsets
    /// </summary>
    public void SetWorkoutOffset()
    {
        Summary sessionSummary = currentSession.summary;
        RiderController currentRider = null;
        for (int i = 0; i < 16; i++)
        {
            switch (currentSession.sessionType)
            {
                case SessionType.individual:
                    currentRider = individualRiders[i];

                    break;
                case SessionType.head2head:
                    currentRider = headToHeadRiders[i];
                    break;
            }
            sessionSummary.sessionWhGenerated[i] = currentRider.bike.whGenerated;
            sessionSummary.peakOutputPowers[i] = currentRider.bike.peakPower;
        }
    }

    public void SetSummary()
    {
        Debug.Log("SetSummary()");
        Summary sessionSummary = currentSession.summary;
        switch (currentSession.sessionType)
        {
            case SessionType.individual:
                //sets session generated power
                sessionSummary.totalWhGenerated = totalGeneratedWh;
                for (int i = 0; i < currentSession.summary.riderRanks.Length; i++)
                {
                    RiderController currentRider = individualRiders[i];
                    sessionSummary.riderRanks[i] = currentRider.rank;
                    sessionSummary.sessionWhGenerated[i] = currentRider.bike.whGenerated - sessionSummary.sessionWhGenerated[i];
                    //sessionSummary.avgOutputPowers[i] /= (spdMax/3)*2;
                    sessionSummary.peakOutputPowers[i] = currentRider.bike.peakPower;

                }

                break;
            case SessionType.head2head:
                for (int i = 0; i < currentSession.summary.teamTotalWhGenerated.Length; i++)
                {
                    float teamTotalWhGenerated = 0;
                    int si = i * 4;
                    int sim = (i + 1) * 4;
                    for (int sii = si; sii < sim; sii++)
                    {
                        RiderController currentRider = headToHeadRiders[sii];

                        sessionSummary.sessionWhGenerated[sii] = currentRider.bike.whGenerated - sessionSummary.sessionWhGenerated[sii];
                        sessionSummary.peakOutputPowers[sii] = currentRider.bike.peakPower;
                        teamTotalWhGenerated += sessionSummary.sessionWhGenerated[sii];
                    }
                    sessionSummary.teamTotalWhGenerated[i] = teamTotalWhGenerated;
                }
                float tempRank;
                float[] temp = CopyFloatArray(sessionSummary.teamTotalWhGenerated);
                foreach (var rank in currentSession.summary.teamRanks)
                {
                    for (int i = currentSession.summary.teamRanks.Length - 1; i > 0; i--)
                    {
                        if (temp[i] > temp[i - 1])
                        {
                            tempRank = temp[i];
                            temp[i] = temp[i - 1];
                            temp[i - 1] = tempRank;
                        }
                    }
                }

                for (int i = 0; i < sessionSummary.teamTotalWhGenerated.Length; i++)
                {
                    sessionSummary.teamRanks[i] = Array.IndexOf(temp, sessionSummary.teamTotalWhGenerated[i]) + 1;
                }
                break;
        }
    }

    public void EndSession()
    {
        // Turn off all pause buttons
        pauseButtons[0].SetActive(false);
        pauseButtons[1].SetActive(false);
        pauseButtons[2].SetActive(false);
        sessionStarted = false;
        SerialWriteReset(1);
        SerialWriteReset(2);
    }

    #endregion

    #region Serial Write Functions
    public void SetResistanceMode(int value)
    {
        resistanceMode = value;
    }
    public void SerialWriteReset(int code)
    {
        dataHandler.WriteToSerial(0, 0, code);
        GameObject serialTimer = GameObject.Instantiate(timer.gameObject);
        Timer time = serialTimer.GetComponent<Timer>();
        time.OnTimerEnd = () =>
        {
            dataHandler.WriteToSerial(0, 0, 0);
            Destroy(serialTimer);
        };
        time.SetTimer(0.1f);
        time.StartTimer();
    }

    public void SerialWrite()
    {
        dataHandler.WriteToSerial(resistanceMode, resistance, 0);
    }

    #endregion

    #region Settings Functions

    public void ChangeBatteryOffset(string text)
    {
        float.TryParse(text, out otdOffset);
    }

    public void AdjustMaxEffort(string text)
    {
        int.TryParse(text, out maximumEffort);
    }

    public void SetHeadToHeadSliderMax(string text)
    {
        int.TryParse(text, out teamSliderMax);
    }

    public void InputFieldConfirm(InputField input)
    {
        input.onEndEdit.Invoke(input.text);
    }

    #endregion

    #region Summary
    /// <summary>
    /// This displays the summary of a session
    /// </summary>
    public void DisplaySummary(bool sessionEnd)
    {
        Debug.Log($"DisplaySummary({sessionEnd})");
        //Ends session
        //sessionStarted = false;

        //Sets summary panels gameobject to true
        summary.gameObject.SetActive(true);
        Summary sessionSummary = currentSession.summary;

        if (sessionEnd)
            currentSession.summary.totalWhGenerated = totalGeneratedWh;

        //Runs code depending on the session type
        switch (currentSession.sessionType)
        {
            case SessionType.freeride:
                //Display session generated power
                summary.freeRidePanel.powerGeneratedText.text = $"{Math.Round(sessionSummary.totalWhGenerated, 2)}Wh";
                //Sets free ride screen to invisible
                //freeRideScreen.SetActive(false);
                //Shows freeride summary screen
                summary.freeRidePanel.panel.SetActive(true);
                break;
            case SessionType.head2head:

                summary.headToHeadRidePanel.teamOneName.text = currentSession.teamNames[0];
                summary.headToHeadRidePanel.teamTwoName.text = currentSession.teamNames[1];
                summary.headToHeadRidePanel.teamThreeName.text = currentSession.teamNames[2];
                summary.headToHeadRidePanel.teamFourName.text = currentSession.teamNames[3];

                summary.headToHeadRidePanel.teamOneTotalPower.text = $"{Math.Round(sessionSummary.teamTotalWhGenerated[0], 1)}Wh";
                summary.headToHeadRidePanel.teamTwoTotalPower.text = $"{Math.Round(sessionSummary.teamTotalWhGenerated[1], 1)}Wh";
                summary.headToHeadRidePanel.teamThreeTotalPower.text = $"{Math.Round(sessionSummary.teamTotalWhGenerated[2], 1)}Wh";
                summary.headToHeadRidePanel.teamFourTotalPower.text = $"{Math.Round(sessionSummary.teamTotalWhGenerated[3], 1)}Wh";

                for (int i = 0; i < summary.headToHeadRidePanel.rankImages.Length; i++)
                {
                    // If the session team rank isn't below 0
                    if (sessionSummary.teamRanks[i] - 1 >= 0)
                        summary.headToHeadRidePanel.rankImages[i].sprite = rankIcons[sessionSummary.teamRanks[i] - 1];
                }

                for (int i = 0; i < summary.headToHeadRidePanel.riders.Length; i++)
                {
                    SummaryRider sRider = summary.headToHeadRidePanel.riders[i];

                    sRider.riderName.text = currentSession.riderNames[i];
                    sRider.sessionPower.text = $"{Math.Round(sessionSummary.sessionWhGenerated[i], 1)}Wh";
                    //sRider.avgPower.text = $"{Math.Round(sessionSummary.avgOutputPowers[i], 2)}W";
                    sRider.maxPower.text = $"{sessionSummary.peakOutputPowers[i]}W";
                }

                summary.headToHeadRidePanel.overallTotalPower.text = $"{Math.Round(sessionSummary.totalWhGenerated, 1)}Wh";

                //Sets head to head ride screen to invisible
                //headToHeadScreen.SetActive(false);
                //Shows head to head ride summary screen
                summary.headToHeadRidePanel.panel.SetActive(true);
                break;
            case SessionType.individual:
                for (int i = 0; i < summary.individualRidePanel.riders.Length; i++)
                {
                    SummaryRider sRider = summary.individualRidePanel.riders[i];
                    Debug.Log(currentSession.riderNames[i] + " is in rank " + (i+1) + " with a session power of " + Math.Round(individualRiders[i].bike.whGenerated, 1) + "Wh and a max power of " + sessionSummary.peakOutputPowers[i] + "W");
                    sRider.riderName.text = currentSession.riderNames[i];
                    sRider.rank.text = sessionSummary.riderRanks[i].ToString().PadLeft(2, '0');
                    sRider.sessionPower.text = $"{Math.Round(sessionSummary.sessionWhGenerated[i], 1)}Wh";
                    //sRider.avgPower.text = $"{Math.Round(sessionSummary.avgOutputPowers[i], 2)}W";
                    sRider.maxPower.text = $"{sessionSummary.peakOutputPowers[i]}W";
                }
                summary.individualRidePanel.overallTotalPower.text = $"{Math.Round(sessionSummary.totalWhGenerated, 1)}Wh";
                //Sets individual ride screen to invisible
                //individualRideScreen.SetActive(false);
                //Shows individual ride summary screen
                summary.individualRidePanel.panel.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// This function sets the current session for Summary purposes
    /// </summary>
    public void SetCurrentSession(int sessionID)
    {
        currentSession = sessions[sessionID];
        DisplaySummary(false);
    }

    /// <summary>
    /// This closes the summary screen and activates the setup screen
    /// </summary>
    public void CloseSummary()
    {
        // Reset the timescalse if it's been changed
        if (Time.timeScale != 1)
            Time.timeScale = 1;

        currentSession.summary.totalWhGenerated = totalGeneratedWh;

        resetOffset = PlayerPrefs.GetFloat("TotalPowerGenerated");
        resetOffset += currentSession.summary.totalWhGenerated;
        PlayerPrefs.SetFloat("TotalPowerGenerated", resetOffset);

        freeRideScreen.SetActive(false);
        headToHeadScreen.SetActive(false);
        individualRideScreen.SetActive(false);
        summaryButton[0].SetActive(false);
        summaryButton[1].SetActive(false);
        summaryButton[2].SetActive(false);
        summary.freeRidePanel.panel.SetActive(false);
        summary.headToHeadRidePanel.panel.SetActive(false);
        summary.individualRidePanel.panel.SetActive(false);
        summary.gameObject.SetActive(false);
        persistentObjects.SetActive(false);
        setupScreen.SetActive(true);
    }
    #endregion

    #region Harry's Code

    /// <summary>
    /// This is used to open the summary screen on button press
    /// </summary>
    public void OpenSummary()
    {
        Debug.Log("OpenSummary()");
        // Prevent the automatic opening of the summary screen
        needToOpen = false;

        // If the timer is still going
        if (timer.GetTimeRemaining() > 0)
            // Show the summary
            DisplaySummary(false);
        // If the timer is still going
        else
            // Show the summary
            DisplaySummary(true);
    }

    /// <summary>
    /// This is used to determine which session from the array list is going to be affected by the changes in the edit menu
    /// </summary>
    public void SetSessionNumber(int SessionNumber)
    {
        // Assign the number
        sessionToEdit = SessionNumber;
    }

    /// <summary>
    /// This is used to get all text components that need to be changed in the setup screen when using the edit menu
    /// </summary>
    public void GetSetupText(GameObject Panel)
    {
        // Grab the session text - child 5
        setupSessionText = Panel.transform.GetChild(5).GetComponent<Text>();
    }

    /// <summary>
    /// This changes the edit menu's options based on which dropdown selection is taken
    /// </summary>
    public void EditMenuSessionDropdown(Int32 SelectedMenu)
    {
        // Temp references to the children of the edit menu needed for changing session - child 4/5/6
        GameObject freerideMenu = editMenuScreen.transform.GetChild(4).gameObject;
        GameObject headtoheadMenu = editMenuScreen.transform.GetChild(5).gameObject;
        GameObject individualMenu = editMenuScreen.transform.GetChild(6).gameObject;

        // Swicth based on the selected menu (0-2)
        switch (SelectedMenu)
        {
            // Free ride
            case 0:
                // Activate the menus
                freerideMenu.SetActive(true);
                headtoheadMenu.SetActive(false);
                individualMenu.SetActive(false);
                // Change the setup menu text
                setupSessionText.text = "Free Ride";
                // Update the session SO
                sessions[sessionToEdit].sessionType = SessionType.freeride;
                break;
            // Head to head
            case 1:
                // Activate the menus
                freerideMenu.SetActive(false);
                headtoheadMenu.SetActive(true);
                individualMenu.SetActive(false);
                // Change the setup menu text
                setupSessionText.text = "Head to Head";
                // Update the session SO
                sessions[sessionToEdit].sessionType = SessionType.head2head;
                break;
            // Individual
            case 2:
                // Activate the menus
                freerideMenu.SetActive(false);
                headtoheadMenu.SetActive(false);
                individualMenu.SetActive(true);
                // Change the setup menu text
                setupSessionText.text = "Individual";
                // Update the session SO
                sessions[sessionToEdit].sessionType = SessionType.individual;
                break;
            // Error
            default:
                break;
        }
    }

    /// <summary>
    /// This is used to determine which rider needs to have their name updated
    /// </summary>
    public void EditMenuConfirmRiderNumber(int RiderNumber)
    {
        // Set the number
        riderNumber = RiderNumber;
    }

    /// <summary>
    /// This is used to update the session SO for a rider's name
    /// </summary>
    public void EditMenuConfirmRiderName(Text TypedInData)
    {
        // Set the string to the typed in name from the text field
        sessions[sessionToEdit].riderNames[riderNumber] = TypedInData.text;
    }

    /// <summary>
    /// This is used to determine which rider needs to have their name updated
    /// </summary>
    public void EditMenuConfirmTeamNumber(int TeamNumber)
    {
        // Set the number
        teamNumber = TeamNumber;
    }

    /// <summary>
    /// This is used to update the session SO for a rider's name
    /// </summary>
    public void EditMenuConfirmTeamName(Text TypedInData)
    {
        // Set the string to the typed in name from the text field
        sessions[sessionToEdit].teamNames[teamNumber] = TypedInData.text;
    }

    /// <summary>
    /// This is used to update the session SO's countdown timer
    /// </summary>
    public void EditSessionLength(string text)
    {
        // Parse the typed number into the timer - will generate an error if not an integer
        float.TryParse(text, out sessions[sessionToEdit].timerLength);
    }

    /// <summary>
    /// This is used to update the session SO's countdown timer
    /// </summary>
    public void EditResistanceValue(string text)
    {
        // Parse the typed number into the timer - will generate an error if not an integer
        int.TryParse(text, out sessions[sessionToEdit].resistanceValue);
    }

    /// <summary>
    /// This is used to display the correct sessions, based on the SO, at the start of running the program
    /// </summary>
    void DisplaySessionTypes()
    {
        // Loop through the session types and set them based on the session SO's session types
        for (int i = 0; i < sessionTypeTexts.Length; i++)
        {
            // Switch based on the session type
            switch (sessions[i].sessionType)
            {
                // Free ride
                case SessionType.freeride:
                    // Set the text
                    sessionTypeTexts[i].text = "Free Ride";
                    break;
                // Individual
                case SessionType.individual:
                    // Set the text
                    sessionTypeTexts[i].text = "Individual";
                    break;
                // Head to head
                case SessionType.head2head:
                    // Set the text
                    sessionTypeTexts[i].text = "Head-To-Head";
                    break;
            }
        }
    }

    /// <summary>
    /// This is used to collect the hide UI button needed to toggle the UI on and off
    /// </summary>
    /// <param name="HideUIButton">The button needed to hide the UI</param>
    public void GetHideIOButton(GameObject HideUIButton)
    {
        // Assign the reference
        hideUIButton = HideUIButton;
    }

    /// <summary>
    /// Use this to toggle the UI of a given session on or off
    /// </summary>
    public void ToggleUI(int NumberOfElements)
    {
        // Set the parent gameobject
        Transform parent = hideUIButton.transform.parent;

        // Loop based on the number of UI elements to disable
        for (int i = 0; i < NumberOfElements; i++)
        {
            // Toggle the UI element on/off
            parent.GetChild(i).gameObject.SetActive(!parent.GetChild(i).gameObject.activeSelf);
        }

        // Toggle the persistent UI object on/off
        persistentObjects.SetActive(!persistentObjects.activeSelf);
        // Toggle the debug settings button on/off
        debugSettingsButton.SetActive(!debugSettingsButton.activeSelf);
    }



    /// <summary>
    /// This is used to trigger the rank change animation on any given rider panel
    /// </summary>
    /// <param name="RiderPanel">Needs each rider that is required to be swapped. Use two seperate calls</param>
    public void TriggerAnimation(GameObject RiderPanel)
    {
        // Create a temp reference to the animator
        Animator animator = RiderPanel.GetComponent<Animator>();
        // Create a temp location to store where the rider panels are
        Vector3 position = RiderPanel.transform.position;

        // If the object is on the right
        if (RiderPanel.transform.position.x >= 625)
        {
            // Set the controller
            animator.runtimeAnimatorController = riderPanelControllerRight;
        }
        // Else if the object is on the right
        else
        {
            // Set the controller
            animator.runtimeAnimatorController = riderPanelControllerLeft;
        }

        // Start the transition coroutine
        StartCoroutine(AnimationDelay(animator, RiderPanel, position));
    }


    /// <summary>
    /// This is used to play out the animation through all of it's transitions
    /// </summary>
    IEnumerator AnimationDelay(Animator Controller, GameObject RiderPanel, Vector3 Position)
    {
        // Add the current panel to the ignore list
        animationIgnoreList.Add(RiderPanel);
        // Play the animation
        Controller.SetBool("StartTransition", true);
        // Wait for the animation to play
        yield return new WaitForSeconds(0.42f);

        // Play the reverse animation
        Controller.SetBool("PlayReverse", true);

        // Wait for the animation to play
        yield return new WaitForSeconds(0.42f);

        // Set the transition bools false
        Controller.SetBool("StartTransition", false);
        Controller.SetBool("PlayReverse", false);

        // Make sure the panel is back in place
        RiderPanel.transform.position = Position;

        // Remove the controller from the animator
        Controller.runtimeAnimatorController = null;
        // Remove the rider from the ignore list
        animationIgnoreList.Remove(RiderPanel);
    }

    #endregion

    [ContextMenu("Reset")]
    void ResetPlayerPrefs()
    {
        PlayerPrefs.SetFloat("TotalPowerGenerated", 0);
    }
}
