using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFace : CutsceneBehaviour
{
    public Transform target;
    public float time;
    public Transform character;
    public override void Play()
    {

        //characterMovement.DoFaceTowards(target.position, time);
        StartCoroutine(FaceTowards(target.position, time));
        StartCoroutine(WaitForComplete(time));
    }

    private IEnumerator WaitForComplete(float time)
    {
        yield return new WaitForSeconds(time);
        OnComplete.Invoke();
    }

    private IEnumerator FaceTowards(Vector3 target, float time)
    {
        
        float startTime = Time.time;
        float endTime = startTime + time;
        float startAngle = character.eulerAngles.y;
        Vector3 groundTarget = new Vector3(target.x, 0, target.z);
        Vector3 groundPosition = new Vector3(character.position.x, 0, character.position.z);
        float targetAngle = Vector3.SignedAngle(Vector3.forward, groundTarget - groundPosition, Vector3.up);

        while (Time.time <= endTime)
        {
            float angle = Mathf.LerpAngle(startAngle, targetAngle, Mathf.InverseLerp(startTime, endTime, Time.time));
            character.eulerAngles = new Vector3(character.eulerAngles.x, angle, character.eulerAngles.z);
            yield return null;
        }

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
