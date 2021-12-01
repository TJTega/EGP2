using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class Data
{
    /// <summary>
    /// This variable holds the port the data is being received from
    /// </summary>
    public SerialPort Stream;
    /// <summary>
    /// This variable holds the raw data read from the port
    /// </summary>
    public string rawData;
    /// <summary>
    /// This holds thread data for the reading data
    /// </summary>
    public Thread readThread;
    /// <summary>
    /// This holds thread data for the reading data
    /// </summary>
    public Thread writeThread;
    [HideInInspector]
    public int resistanceMode = 0, resistance = 0, resetCode = 0;

    public Data(SerialPort stream)
    {
        Stream = stream;
        stream.Open();
    }

    //Called once per frame
    public void SerialRead()
    {
        while(Stream.IsOpen)
        {
            rawData = Stream.ReadLine();
            Stream.BaseStream.Flush();
        }
    }

    //Called once per frame
    public void SerialWrite()
    {
        while (Stream.IsOpen)
        {
            Stream.WriteLine($"98/{resistanceMode}/{resistance}/{resetCode}/76");
        }
    }
}
