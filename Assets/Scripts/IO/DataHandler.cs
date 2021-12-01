using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.Events;
using System.Threading;
using UnityEngine.UI;

public enum ReadType
{
    instant,
    continuous,
    raw
}

public class DataHandler : MonoBehaviour
{
    /// <summary>
    /// This is the class handling the incoming data from Serial
    /// </summary>
    Data data;

    /// <summary>
    /// This variable holds the port name for where the data should be read from
    /// </summary>
    public string port = "COM7"; //"COM7" is just a default value
    /// <summary>
    /// This variable holds the port the data is being received from
    /// </summary>
    public SerialPort stream;


    /// <summary>
    /// This variable holds mock data representing the microcontroller input
    /// </summary>
    [SerializeField]
    private string mockData = "B1/426/728/23/2578/45/98/0,B2/426/728/23/2578/45/98/0,B3/426/728/23/2578/45/98/0,B4/426/728/23/2578/45/98/0,B5/426/728/23/2578/45/98/0,B6/426/728/23/2578/45/98/0,B7/426/728/23/2578/45/98/0,B8/426/728/23/2578/45/98/0,B9/426/728/23/2578/45/98/0,B10/426/728/23/2578/45/98/0,B11/426/728/23/2578/45/98/0,B12/426/728/23/2578/45/98/0,B13/426/728/23/2578/45/98/0,B14/426/728/23/2578/45/98/0,B15/426/728/23/2578/45/98/0,B16/426/728/23/2578/45/98/0,B17/426/728/23/2578/45/98/0,LINE END INDICATOR";

    /// <summary>
    /// This holds data for each of the bikes connected
    /// </summary>
    public Bike[] bikes = new Bike[17];

    /// <summary>
    /// This controls the read type for debug purposes
    /// </summary>
    public ReadType readType;

    [HideInInspector]
    public int resMod, res, resCode;

    public Text portText;

    /*/// <summary>
    /// This is called on a port error
    /// </summary>
    //public UnityEvent onPortError;

    /// <summary>
    /// This holds reference to the error shown on port error
    /// </summary>
    public GameObject portNotFoundError;*/


    private void Awake()
    {
        //Sets data stream
        SetStream();
    }

    /// <summary>
    /// This sets the data stream and initializes a thread
    /// </summary>
    private void SetStream()
    {
        portText.text = port;
        if (port != "COM1")
        {
            //Set new data stream form port
            stream = new SerialPort(port, 38400);
            //Initialise new data class from the stream
            data = new Data(stream);
            //Set a read thread
            data.readThread = new Thread(new ThreadStart(data.SerialRead));
            //Set a write thread
            data.writeThread = new Thread(new ThreadStart(data.SerialWrite));
            //Start the read thread
            data.readThread.Start();
            //Start the write thread
            data.writeThread.Start();
        }
    }

    /// <summary>
    /// This changes the port of the data stream
    /// </summary>
    public void ChangePort()
    {
        //Stops the thread if it is set
        if (data != null)
        {
            data.readThread.Abort();
            data.writeThread.Abort();
        }
        //Closes the serial port
        stream.Dispose();
        //Resets stream with new port
        SetStream();
    }

    /// <summary>
    /// Takes raw data and splits into declared variables
    /// </summary>
    /// <param name="data">Raw data input</param>
    public void ReadData(string data)
    {
        //Splits data by ','
        string[] dataSets = data.Split(',');
        //Checks the length of the data is correct
        if (dataSets.Length == 18)
        {
            //For each bike
            for(int i = 0; i < 17; i++)
            {
                //Split bike data by '/'
                string[] values = dataSets[i].Split('/');
                //Sets the current bike
                Bike current = bikes[i];

                //Sets bike data
                current.bikeNo = i+1;
                current.wattsInstant = int.Parse(values[1]);
                current.peakPower = int.Parse(values[2]);
                current.whGenerated = float.Parse(values[3]) / 100;
                current.vbatVoltage = (float.Parse(values[4])*2)/100;
                current.ouputCurrent = float.Parse(values[5])/10;
                current.cadenceRPM = int.Parse(values[6]);
                current.errorCode = int.Parse(values[7]);
                
            }
        }
        //Returns error if data doesn't match
        else if (dataSets.Length != 18)
        {
            Debug.LogError("Input data length does not match format!");
        }
    }

    //Called once per frame
    private void Update()
    {
        //This switches between instant, continous and raw data reading
        if (readType == ReadType.continuous)
        {
            //portNotFoundError.SetActive(false);
            ReadData(RandomData());
        }
        else if (readType == ReadType.raw)
        {

            if (stream.IsOpen)
            {
                //portNotFoundError.SetActive(false);
                //Passes data into ReadData() function
                if (data.rawData != null)
                {
                    ReadData(data.rawData);
                    Debug.Log(data.rawData);
                }

                //Debug.Log(data.rawData);
            }
            else
            {
                Debug.LogError("Error: Data stream not set!");
                //onPortError.Invoke();
                //Sets read type to instant so error is only called once
                readType = ReadType.instant;
            }
        }
        else if (readType == ReadType.instant)
        {
            //portNotFoundError.SetActive(false);
            ReadData(mockData);
        }
    }

    /// <summary>
    /// This function writes to Serial
    /// </summary>
    /// <param name="resistanceMode">This is the mode of .... 0 is battery mode and 1 is resistance mode</param>
    /// <param name="resistance">This is the resistance being pushed back to the bike</param>
    /// <param name="resetCode">This tells the bike which parameters to reset</param>
    public void WriteToSerial(int resistanceMode, int resistance, int resetCode)
    {
        if (data != null)
        {
            data.resistanceMode = resistanceMode;
            data.resistance = resistance;
            data.resetCode = resetCode;
        }
    }

    /// <summary>
    ///  Sets mock data to a snapshot of random values
    /// </summary>
    [ContextMenu("Set Mock Data")]
    private void SetMockData()
    {
        mockData = RandomData();
    }

    /// <summary>
    /// Outputs a set of random values in the input data format
    /// </summary>
    /// <returns>A string containing mock data</returns>
    private string RandomData()
    {
        //Variable setup
        string data_1;
        int data_2;
        int data_3;
        float data_4;
        float data_5;
        float data_6;
        int data_7;
        int data_8;

        string output = "";

        //For each virtual bike
        for (int i = 1; i < 18; i++)
        {
            //Create random data
            data_1 = $"B{i}";
            data_2 = Random.Range(0, 2001);
            data_3 = Random.Range(0, 2001);
            data_4 = Random.Range(0, 4001);
            data_5 = Random.Range(0, 4001);
            data_6 = Random.Range(0, 256);
            data_7 = Random.Range(0, 256);
            data_8 = /*Random.Range(0, 4);*/ 0;

            //Add this data to ouput
            output += string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7},", data_1, data_2, data_3, data_4, data_5, data_6, data_7, data_8);
        }
        return output;
    }
}