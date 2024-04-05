using System;
using Unity.MLAgents.Sensors;
using UnityEngine;


[AddComponentMenu("Connect Four Agents/Connect Four Sensor", 50)]
public class ConnectFourSensorComponent: SensorComponent, IDisposable
{
    [HideInInspector, SerializeField]
    string m_SensorName = "Connect Four Sensor";
    
    public string SensorName
    {
        get => m_SensorName;
        set => m_SensorName = value;
    }
    
    private ISensor[] m_Sensors;
    
    public override ISensor[] CreateSensors()
    {
        // Clean up any existing sensors
        Dispose();

        var agent = GetComponent<ConnectFourAgent>();
        if (!agent)
        {
            return Array.Empty<ISensor>();
        }
        
        var boardSensor =  new ConnectFourSensor(agent.Logic, "Connect Four Sensor");
        Debug.Log("Creating Sensor");
        return new ISensor[] { boardSensor };
    }

    public void Dispose()
    {
        if (m_Sensors != null)
        {
            for (var i = 0; i < m_Sensors.Length; i++)
            {
                ((ConnectFourSensor)m_Sensors[i]).Dispose();
            }
            m_Sensors = null;
        }
    }
}
