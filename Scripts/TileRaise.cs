using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRaise : MonoBehaviour
{
    [SerializeField]
    private float lowOffset = -0.1f;
    [SerializeField]
    private float timeToRaise = 0.15f;
    [SerializeField]
    private float timeToLower = 0.25f;
    [SerializeField]
    private bool isUp = false;
    private Vector3 upPosition;
    private Vector3 downPosition;

    FMOD.Studio.EventInstance SFX_Objects_Tiles_Move;
    private void Raise()
    {
        SFX_Objects_Tiles_Move = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_Objects/SFX_Objects_Tiles_Move");
        SFX_Objects_Tiles_Move.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        SFX_Objects_Tiles_Move.start();
        StartCoroutine(MoveTo(upPosition, timeToRaise));
        isUp = true;
    }

    private void Lower()
    {
        SFX_Objects_Tiles_Move = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_Objects/SFX_Objects_Tiles_Move");
        SFX_Objects_Tiles_Move.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        SFX_Objects_Tiles_Move.start();
        StartCoroutine(MoveTo(downPosition, timeToLower));
        isUp = false;
    }

    private IEnumerator MoveTo(Vector3 target, float time)
    {
        float start = Time.time;
        float end = Time.time + time;
        Vector3 startPos = transform.position;
        while(Time.time <= end)
        {
            transform.position = Vector3.Lerp(startPos, target, Mathf.InverseLerp(start, end, Time.time));
            yield return null;
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
        upPosition = transform.position;
        downPosition = transform.position + Vector3.up * lowOffset;
        if (!isUp)
            transform.position = downPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
