using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Yarn.Unity;

public class CharacterPlayAnim : MonoBehaviour
{
    private Animator animator;
    [YarnCommand("playanim")]
    public void PlayAnim(string state)
    {
        Debug.Log("playanim " + state);
        animator.Play(state);
    }
    [YarnCommand("triggeranim")]
    public void TriggerAnim(string trigger)
    {
        Debug.Log("triggeranim " + trigger);
        animator.SetTrigger(trigger);
    }
    [YarnCommand("playtimeline")]
    public void PlayTimeline()
    {
        GetComponent<PlayableDirector>().Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
