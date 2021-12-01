using UnityEngine;

[CreateAssetMenu(fileName = "Bike", menuName = "Arcade/Bike", order = 0)]
public class Bike : ScriptableObject
{
    [Header("Meta")]

    ///<summary> This is the number of the bike </summary>
    public int bikeNo;
    /// <summary>
    /// This is the name of the current rider on this bike
    /// </summary>
    public string riderName;
    /*/// <summary>
    /// 
    /// </summary>*/
    public int wattsInstant;
    /*/// <summary>
    /// 
    /// </summary>*/
    public int peakPower;
    /*/// <summary>
    /// 
    /// </summary>*/
    public float whGenerated;
    /*/// <summary>
    /// 
    /// </summary>*/
    public float vbatVoltage;
    /*/// <summary>
    /// 
    /// </summary>*/
    public float ouputCurrent;
    /*/// <summary>
    /// 
    /// </summary>*/
    public int cadenceRPM;
    /*/// <summary>
    /// 
    /// </summary>*/
    public int errorCode;
}
