using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    
    public List<FabricSO> fabrics;
    public GameObject movementReceiver;
    public GameObject dialogueReceiver;

    private IInputReceiver currentReciever;

    private bool[] oldKeyStates;


    public void SwitchToMovement()
    {
        IInputReceiver rec = movementReceiver.GetComponent<IInputReceiver>();
        if (rec == null) return;
        currentReciever = rec;
    }
    public void SwitchToDialogue()
    {
        IInputReceiver rec = dialogueReceiver.GetComponent<IInputReceiver>();
        if (rec == null) return;
        currentReciever = rec;
    }

    public void SwitchTo(IInputReceiver newReciever)
    {
        currentReciever = newReciever;

    }

    public void SwitchToNone()
    {
        currentReciever = null;
    }

    public FabricSO GetFabric(string tag)
    {
        
        foreach(FabricSO fabric in fabrics)
        {
            if (fabric.tag == tag)
                return fabric;
        }
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        oldKeyStates = new bool[fabrics.Count];
        currentReciever = movementReceiver.GetComponent<IInputReceiver>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FabricSO fabric in fabrics)
        {
            string s = fabric.key;
            string keypressed = s.Substring(0, 1).ToLower();
            if (Input.GetKeyDown(keypressed))
            {
                SendKeyPress(fabric);
            }
        }
    }

    void SendKeyPress(FabricSO fabric)
    {
        if (currentReciever == null) return;
        currentReciever.KeyPressed(fabric);
        
    }

    void SendAnalog(string k, int value)
    {
        if (currentReciever == null) return;
        currentReciever.MaxAnalogValue(k, value);

    }

    // Ardity serial events

    void OnMessageArrived(string msg)
    {
        string[] substrings = msg.Split('\t');
        
        for (int i = 0; i < fabrics.Count; i++)
        {
            int value = -1;
            if (!int.TryParse(substrings[0].Substring(i, 1), out value))
                continue;
            if (value == 0 && !oldKeyStates[i])
            {
                SendKeyPress(fabrics[i]);
                Debug.Log("controller buttin " + i);
                oldKeyStates[i] = true;
            }
            if (value == 1)
            {
                oldKeyStates[i] = false;
            }
        }
        return; // TODO FIX CAPACITIVE
        /*
        int maxValue = -1;
        string maxKey = "";
        for (int i = 0; i < keys.Count; i++)
        {
            int value = -1;
            if (!int.TryParse(substrings[i+1], out value))
                continue;

            if (value > maxValue)
            {
                maxValue = value;
                maxKey = keys[i];
            }
        }*/
    }

    void OnConnectionEvent(bool success)
    {

    }
}


public interface IInputReceiver
{
    public void KeyPressed(FabricSO fabric);

    public void MaxAnalogValue(string k, int value);
}
