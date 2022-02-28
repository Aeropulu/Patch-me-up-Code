using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimHead : MonoBehaviour
{
    public float maxDelay = 2.0f;
    public float period = 2.0f;

    private float nextTime;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        nextTime = Time.time + Random.value * maxDelay;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTime)
        {
            nextTime += period;
            rectTransform.localEulerAngles = new Vector3(0, 0, -rectTransform.localEulerAngles.z);
        }
    }
}
