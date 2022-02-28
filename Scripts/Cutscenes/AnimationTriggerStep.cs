using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationTriggerStep : CutsceneBehaviour
{
    public UnityEvent OnStart;
    public float duration = 2.0f;

    public override void Play()
    {
        OnStart.Invoke();
        
        StartCoroutine(WaitForEnd(duration));
    }

    private IEnumerator WaitForEnd(float time)
    {
        yield return new WaitForSeconds(time);
        
        OnComplete.Invoke();
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
