using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputReceiver : MonoBehaviour, IInputReceiver
{
    public List<MenuButton> mainButtons;
    public List<MenuButton> languageButtons;

    private List<MenuButton> buttons;
    private MenuButton buttonSelected = null;
    public void KeyPressed(FabricSO fabric)
    {
        Debug.Log("menu received : " + fabric.name);
        MenuButton buttonPressed = buttons.Find(b => b.fabric == fabric);
        if (buttonPressed == null)
            return;
        if (buttonSelected != null)
            buttonSelected.SetSelected(false);

        if (buttonPressed == buttonSelected)
        {
            buttonSelected.Press();
            buttonSelected = null;
        }
        else
        {
            buttonSelected = buttonPressed;
            buttonSelected.SetSelected(true);
        }
    }

    public void MaxAnalogValue(string k, int value)
    {
        throw new System.NotImplementedException();
    }

    public void SwitchButtons()
    {
        
        if (buttons == mainButtons)
            buttons = languageButtons;
        else
            buttons = mainButtons;
    }

    // Start is called before the first frame update
    void Start()
    {
        buttons = mainButtons;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
