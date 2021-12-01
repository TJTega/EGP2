using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadToHeadPanelManager : MonoBehaviour
{
    public Text timer;
    public Text sessionPhase;
    public Image[] positions;
    public Text[] teamNames = new Text[4];
    public Image[] teamPowerDisplays = new Image[4];
    public Text[] teamPowerQuantityDisplay = new Text[4];
}
