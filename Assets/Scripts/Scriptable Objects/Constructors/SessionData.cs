using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Summary
{
    [Header("Free Ride")]
    /// <summary>
    /// This holds the total Wh generated for the session
    /// </summary>
    public float totalWhGenerated;

    [Header("Individual Ride")]
    public int[] riderRanks = new int[16];
    public float[] sessionWhGenerated = new float[16];
    public float[] avgOutputPowers = new float[16];
    public int[] peakOutputPowers = new int[16];

    [Header("Head To Head Ride")]
    public float[] teamTotalWhGenerated = new float[4];
    public int[] teamRanks = new int[4];
}
[CreateAssetMenu(fileName = "NewSession", menuName = "Arcade/Session", order = 0)]
public class SessionData : ScriptableObject
{
    [Header("Setup")]
    /// <summary>
    /// This holds the value for the session timer length
    /// </summary>
    public float timerLength = 30;
    /// <summary>
    /// This holds the type of session data is being held for
    /// </summary>
    public SessionType sessionType;
    /// <summary>
    /// This holds the names of each team - this should only be filled in for the head to head mode
    /// </summary>
    public string[] teamNames = new string[4];
    /// <summary>
    /// This holds the names of each rider - this should only be filled in for non-free ride sessions
    /// </summary>
    public string[] riderNames = new string[16];
    /// <summary>
    /// This holds the resistance value to use for the workout session - this should only be used for free ride sessions
    /// </summary>
    public int resistanceValue = 4;
    /// <summary>
    /// This holds the resistance pattern for the ride
    /// </summary>
    public Dictionary<float/*time*/, int/*resistance*/> resistancePattern = new Dictionary<float, int>
    {
        // Warm Up
         { 30, 4}, { 29.5f, 4}, { 29, 4}, { 28.5f, 4}, { 28f, 4}, { 27.5f, 4}, { 27, 4}, { 26.5f, 4}, { 26, 4}, { 25.5f, 4},
        // Workout
         { 25, 4}, { 24.5f, 6}, { 24, 8}, { 23.5f, 6}, { 23, 4}, { 22.5f, 6}, { 22, 6}, { 21.5f, 6}, { 21, 8}, { 20.5f, 4}, { 20, 4}, { 19.5f, 6}, { 19, 6}, { 18.5f, 6}, { 18, 8}, { 17.5f, 4}, { 17, 4}, { 16.5f, 8}, { 16, 6}, { 15.5f, 8}, { 15, 6}, { 14.5f, 4}, { 14, 4}, { 13.5f, 4}, { 13, 8}, { 12.5f, 6}, { 12, 8}, { 11.5f, 6}, { 11, 4}, { 10.5f, 4}, { 10, 6}, { 9.5f, 6}, { 9, 6}, { 8.5f, 8}, { 8, 4}, { 7.5f, 4}, { 7, 6}, { 6.5f, 6}, { 6, 8}, { 5.5f, 8},
        // Cool Down
         { 5f, 2}, { 4.5f, 2}, { 4, 2}, { 3.5f, 2}, { 3, 2}, { 2.5f, 2}, { 2, 2}, { 1.5f, 2}, { 1, 2}, { 0.5f, 2}


    };

    public Summary summary;

}
