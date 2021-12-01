using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConflictAvoidance : MonoBehaviour
{
    /// <summary>
    /// This is used to store and remove bikes that generate errors 
    /// </summary>
    public List<string> errorBikes = new List<string>();
    /// <summary>
    /// This is used to hold the reference to the error message text
    /// </summary>
    public Text errorMessage;
    /// <summary>
    /// This is used through the debug menu to prevent/allow the error message to appear
    /// </summary>
    public bool disableErrorMessage;
    /// <summary>
    /// This is a refernece to the data handler
    /// </summary>
    public DataHandler dataHandler;
    /// <summary>
    /// This is an event to trigger the error message
    /// </summary>
    public UnityEvent onErrorMessage;



    /// <summary>
    /// This is used by a button in the debug menu to allow for the error message to be hidden when needed
    /// </summary>
    public void DisableErrorMessage(Text ButtonText)
    {
        // Toggle the ability for the error message to appear on/off
        disableErrorMessage = !disableErrorMessage;

        // If the error message should display
        if (!disableErrorMessage)
            // Set the text to enabled
            ButtonText.text = "Enabled";
        // Else if the error message should not display
        else
            // Set the text to disabled
            ButtonText.text = "Disabled";
    }

    /// <summary>
    /// This is used by a button in the debug menu to clear all previous error messages
    /// </summary>
    public void ResetErrorMessage()
    {
        // If the error bikes list isn't empty
        if (errorBikes.Count > 0)
            // Clear the list
            errorBikes.Clear();

        // If the error message is still showing
        if (errorMessage.gameObject.activeSelf)
            // Turn the error messgae off
            errorMessage.gameObject.SetActive(false);

    }

    private void Update()
    {
        // Loop through each bike
        foreach (var bike in dataHandler.bikes)
        {
            // Switch based on the error code
            switch (bike.errorCode)
            {
                // If the bike needs to change modes (battery to resistance mode)
                case 1:
                    // Trigger the error message
                    onErrorMessage.Invoke();
                    // Display the error message
                    ShowErrorMessage(bike.bikeNo.ToString());
                    // Break from switch
                    break;
            }
        }
    }

    /// <summary>
    /// This is used to display the error message when one or more bikes stops sending data for 5 seconds or more during an active session
    /// </summary>
    void ShowErrorMessage(string BikeName)
    {
        // If the bike with the error isn't already being display
        if (!errorBikes.Contains(BikeName))
            // Add the bike name to the error bikes
            errorBikes.Add(BikeName);

        // Create a temp string to store the bikes
        string tempName = "";

        // Loop based on the number of error bikes
        for (int i = 0; i < errorBikes.Count; i++)
        {
            // If this is the first loop
            if (i == 0)
                // Assign the first bike name to the string
                tempName = errorBikes[i];
            // Else if this is any loop after
            else
                // Assign each next bike name to the string correctly
                tempName += (", " + errorBikes[i]);
        }

        // Create the error message
        errorMessage.text = "There is an issue with Bike(s) : " + tempName + " : Please investigate the situation!";

        // If the error message isn't disabled
        if (!disableErrorMessage)
            // Turn the error messgae on
            errorMessage.gameObject.SetActive(true);
    }
}