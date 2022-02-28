using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;

public class DialogueInputReciever : MonoBehaviour, IInputReceiver
{
    
    public void KeyPressed(FabricSO fabric)
    {
        string k = fabric.key;
        foreach (OptionView option in GetComponentsInChildren<OptionView>())
        {
            string firstchar = option.GetComponent<TextMeshProUGUI>().GetParsedText().Substring(0, 1);
            if (firstchar == k.ToUpper())
            {
                option.InvokeOptionSelected();
                Debug.Log(k);
            }
        }
    }

    public void MaxAnalogValue(string k, int value)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
