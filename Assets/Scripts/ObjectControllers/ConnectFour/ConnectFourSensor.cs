using Unity.MLAgents.Sensors;
using UnityEngine;

public class ConnectFourSensor : ISensor
{
    private ConnectFourLogic logic;
    private string name;
    private ObservationSpec m_ObservationSpec;

    public ConnectFourSensor(ConnectFourLogic logic, string name)
    {
        this.logic = logic;
        this.name = name;
        m_ObservationSpec = ObservationSpec.Vector(logic.Height * logic.Width);
    }
    
    public ObservationSpec GetObservationSpec()
    {
        Debug.Log("Get Compressed Observation Sensor SPEC");
        return m_ObservationSpec;
    }

    public int Write(ObservationWriter writer)
    {
        int written = 0;
        Debug.Log("logic height: " +logic.Height);
        for (int y = 0; y < logic.Height; y++)
        {
            for (int x = 0; x < logic.Width; x++)
            {
                // !DISABLED THIS PATH
                // Debug.Log(logic.GetBoardAsFloats()[y, x]);
                // Write each cell of the board into the observation writer
                // writer[written] = logic.GetBoardAsFloats()[y, x];
                written++;
            }
        }
        Debug.Log("Write Sensor");
        return written;
    }

    public byte[] GetCompressedObservation()
    {
        // This sensor does not use compressed observations
        Debug.Log("Get Compressed Observation Sensor");
        return null;
    }

    public void Update() { }

    public void Reset() { }

    public CompressionSpec GetCompressionSpec()
    {
        return CompressionSpec.Default();
    }

    public string GetName()
    {
        return name;
    }
    
    public void Dispose()
    {
        Debug.Log("Trying Dispose Sensor");
       //Nothing here??
    }
}