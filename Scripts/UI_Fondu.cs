using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Fondu : MonoBehaviour
{


    public void FadeAlpha(float from, float to, float duration)
    {
        StartCoroutine(DoFadeAlpha(from, to, duration));
    }

    private IEnumerator DoFadeAlpha(float from, float to, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time <= endTime)
        {
            float alpha = Mathf.Lerp(from, to, Mathf.InverseLerp(startTime, endTime, Time.time));
            GetComponent<CanvasGroup>().alpha = alpha;            
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
