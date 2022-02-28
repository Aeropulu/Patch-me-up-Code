using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LanguageStartupSetter : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("language"))
            GetComponent<TextLineProvider>().textLanguageCode = PlayerPrefs.GetString("language");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
