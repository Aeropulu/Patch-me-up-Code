using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class TriggerDialogue : MonoBehaviour
{
    public string dialogueNode;
    public DialogueRunner dialogueRunner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //dialoguebox.SetActive(true);
        
        dialogueRunner.StartDialogue(dialogueNode);
    }

    private void OnTriggerExit(Collider other)
    {
        //dialoguebox.SetActive(false);
        gameObject.SetActive(false);
        dialogueRunner.Stop();
    }
}
