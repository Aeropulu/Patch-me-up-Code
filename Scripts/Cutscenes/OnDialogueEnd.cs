using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDialogueEnd : CutsceneBehaviour
{
    public override void Play()
    {
        FindObjectOfType<DialogueUIManager>().onDialogueStop.AddListener(OnComplete.Invoke);
        FindObjectOfType<DialogueUIManager>().onDialogueStop.AddListener(RemoveFromManager);
    }

    public void RemoveFromManager()
    {
        StartCoroutine(DoRemove());
    }
    private IEnumerator DoRemove()
    {
        yield return null;
        FindObjectOfType<DialogueUIManager>().onDialogueStop.RemoveListener(OnComplete.Invoke);
        FindObjectOfType<DialogueUIManager>().onDialogueStop.RemoveListener(RemoveFromManager);
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
