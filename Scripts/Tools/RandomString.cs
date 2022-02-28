using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class RandomString : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        var comp = GetComponent<UnityEngine.Localization.Components.LocalizeStringEvent>();
        string key = "Test_" + Random.Range(1, 3).ToString();
        comp.StringReference.SetReference("Test Dialogue", key);
    }
}
