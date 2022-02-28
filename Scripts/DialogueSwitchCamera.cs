using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueSwitchCamera : MonoBehaviour
{
    public GameObject cameraToSwitch;
    private DialogueUIManager manager;

    [YarnCommand("SwitchOnCamera")]
    public void SwitchOnCamera()
    {
        cameraToSwitch.SetActive(true);
        manager.onDialogueStop.AddListener(this.SwitchOffCamera);
    }

    [YarnCommand("SwitchOffCamera")]
    public void SwitchOffCamera()
    {
        cameraToSwitch.SetActive(false);
        manager.onDialogueStop.RemoveListener(this.SwitchOffCamera);
    }

    private void Start()
    {
        manager = GameObject.FindObjectOfType<DialogueUIManager>();
    }
}
