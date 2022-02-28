using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class OnEnableEvent : MonoBehaviour
{
    public UnityEvent eventOnEnable;
    private void OnEnable()
    {
        eventOnEnable.Invoke();
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
