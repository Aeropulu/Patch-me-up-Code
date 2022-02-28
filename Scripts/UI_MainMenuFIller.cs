using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainMenuFIller : MonoBehaviour
{
    public string node = "UI_main_menu";
    public List<TMPro.TextMeshProUGUI> textFields;

    public void Fill()
    {
        foreach (var s in textFields)
            GetComponent<UI_MenuFiller>().textFields.Add(s);
        if (PlayerPrefs.HasKey("language"))
            GetComponent<Yarn.Unity.TextLineProvider>().textLanguageCode = PlayerPrefs.GetString("language");
        GetComponent<Yarn.Unity.DialogueRunner>().StartDialogue(node);
    }
    IEnumerator Start()
    {
        yield return null;
        Fill();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
