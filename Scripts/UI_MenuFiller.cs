using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class UI_MenuFiller : DialogueViewBase
{

    public List<TMPro.TextMeshProUGUI> textFields;



    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        Debug.Log(dialogueLine.Text.Text);
        if (textFields.Count > 0)
        {
            textFields[0].text = dialogueLine.TextWithoutCharacterName.Text;
            textFields.RemoveAt(0);
        }
        onDialogueLineFinished();
        
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
