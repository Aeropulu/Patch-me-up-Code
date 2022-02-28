using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SetupSerial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SerialController controller = GetComponentInChildren<SerialController>();
        
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/com port.txt");
        controller.enabled = false;
        controller.portName = reader.ReadLine();
        controller.enabled = true;
    }

    
}
