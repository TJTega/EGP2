using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class holds references for the free ride panel
/// </summary>
[Serializable]
public class FreeRidePanel
{
    /// <summary>
    /// This holds a reference to the gameobject
    /// </summary>
    public GameObject panel;
    /// <summary>
    /// This holds a reference to the power generated field in the UI
    /// </summary>
    public Text powerGeneratedText;
}
/// <summary>
/// This class holds references for the individual ride panel
/// </summary>
[Serializable]
public class IndividualRidePanel
{
    /// <summary>
    /// This holds a reference to the gameobject
    /// </summary>
    public GameObject panel;
    /// <summary>
    /// This holds a reference to the overall total across all the bike in individual ride
    /// </summary>
    public Text overallTotalPower;
    /// <summary>
    /// This holds a reference to riders
    /// </summary>
    public SummaryRider[] riders;
}
/// <summary>
/// This class holds references for the head to head ride panel
/// </summary>
[Serializable]
public class HeadToHeadRidePanel
{
    /// <summary>
    /// This holds a reference to the gameobject
    /// </summary>
    public GameObject panel;
    /// <summary>
    /// This holds a reference to the rank image placeholders in the Head to Head UI
    /// </summary>
    public Image[] rankImages = new Image[4];
    /// <summary>
    /// This holds a reference to the riders
    /// </summary>
    public SummaryRider[] riders;
    /// <summary>
    /// This holds a reference to the overall total across all the bike in head to head
    /// </summary>
    public Text overallTotalPower;
    [Header("Team 1")]
    /// <summary>
    /// This holds a reference to the team one name field in the UI
    /// </summary>
    public Text teamOneName;
    /// <summary>
    /// This holds a reference to the team one total power field in the UI
    /// </summary>
    public Text teamOneTotalPower;
    [Header("Team 2")]
    /// <summary>
    /// This holds a reference to the team two name field in the UI
    /// </summary>
    public Text teamTwoName;
    /// <summary>
    /// This holds a reference to the team two total power field in the UI
    /// </summary>
    public Text teamTwoTotalPower;
    [Header("Team 3")]
    /// <summary>
    /// This holds a reference to the team two name field in the UI
    /// </summary>
    public Text teamThreeName;
    /// <summary>
    /// This holds a reference to the team three total power field in the UI
    /// </summary>
    public Text teamThreeTotalPower;
    [Header("Team 4")]
    /// <summary>
    /// This holds a reference to the team four name field in the UI
    /// </summary>
    public Text teamFourName;
    /// <summary>
    /// This holds a reference to the team four total power field in the UI
    /// </summary>
    public Text teamFourTotalPower;
}
public class SummaryPanelsManager : MonoBehaviour
{
    /// <summary>
    /// This holds a reference to the background object
    /// </summary>
    public GameObject background;
    /// <summary>
    /// This is the references for the free ride summary panel
    /// </summary>
    public FreeRidePanel freeRidePanel;
    /// <summary>
    /// This is the references for the individual ride summary panel
    /// </summary>
    public IndividualRidePanel individualRidePanel;
    /// <summary>
    /// This is the references for the head to head ride summary panel
    /// </summary>
    public HeadToHeadRidePanel headToHeadRidePanel;

}
