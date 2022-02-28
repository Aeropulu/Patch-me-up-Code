using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshStep : CutsceneBehaviour
{
    public float timeout = 10.0f;
    public NavMeshAgent agent;
    public Transform destination;
    public override void Play()
    {
        agent.destination = destination.position;
        agent.isStopped = false;
        StartCoroutine(WaitForEnd(timeout));
    }

    private IEnumerator WaitForEnd(float timeout)
    {
        float start = Time.time;
        float end = start + timeout;
        while (Time.time <= end)
        {
            if (Vector3.Distance(agent.transform.position, agent.destination) <= agent.stoppingDistance)
                break;
            yield return null;
        }
        agent.destination = agent.transform.position;
        agent.isStopped = true;
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
