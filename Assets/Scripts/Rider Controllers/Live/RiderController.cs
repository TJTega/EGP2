using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiderController : MonoBehaviour
{
    public int rank;
    /// <summary>
    /// This holds a reference to the rider name field in UI
    /// </summary>
    public Text riderNameText;
    /// <summary>
    /// This holds reference to the cadence field in UI
    /// </summary>
    public Text instantPowerText;
    /// <summary>
    /// This holds a reference to the output power field in UI
    /// </summary>
    public Text powerOutputText;
    /// <summary>
    /// This holds a reference to the effort meter images in UI
    /// </summary>
    public Image[] effortMeterBars = new Image[3];

    public Bike bike = null;
}
