using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LanguageSelectionButton : MonoBehaviour
{
    [Language]
    public string language;

    public void SwitchTo()
    {

        PlayerPrefs.SetString("language", language);

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
