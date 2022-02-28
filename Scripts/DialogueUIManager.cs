using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Yarn.Unity;
using Yarn.Markup;
using TMPro;

public class DialogueUIManager : DialogueViewBase, IInputReceiver
{
    [SerializeField]
    private float boxPopupTime = 0.2f;
    [SerializeField]
    private float boxFadeOutTime = 0.5f;
    [SerializeField]
    private int maxBoxes = 4;
    private List<RectTransform> shownBoxes = new List<RectTransform>();

    
    public GameObject linePrefab;
    public GameObject optionPrefab;
    public GameObject neutralOptionPrefab;
    public GameObject optionsSelector;
    public GameObject picturePrefab;
    public GameObject nameBubblePrefab;
    public GameObject fabricUnlockPrefab;

    public DialogueRunner dialogueRunner;
    public DialogueRunner menuRunner;

    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueStop;
    public UnityEvent<int> onPreSelect;
    public UnityEvent<int> onAnswer;
    public UnityEvent<string> onCharacterSpeak;


    public List<CharacterSO> characters;
    public List<FabricSO> availableFabrics;


    private Dictionary<string, DialogueOption> options = new Dictionary<string, DialogueOption>();
    private Dictionary<string, GameObject> optionBoxes = new Dictionary<string, GameObject>();
    private System.Action<int> optionSelected;
    private bool isDialogueEnd = false;
    private bool isDialoguePaused = false;
    private bool isExpectingInput = false;
    private string previousCharacterName = "";
    private string previousPictureName = "";
    private string pictureName = "";
    private string selectedKey = "";

    private bool isAnswer = false; // true on the first line after a choice

    private InputManager inputManager;
    private FabricSO newFabric;

    public override void DialogueComplete()
    {
        isDialogueEnd = true;
        //MakeOptionBox("---", Color.gray, null, true);
        MakeNeutralOptionBox("UI_stop_dialogue");
    }
    public override void DialogueStarted()
    {
        previousCharacterName = "";
        gameObject.SetActive(true);
        onDialogueStart.Invoke();
    }
    public override void DismissLine(System.Action onDismissalComplete)
    {
        onDismissalComplete();
    }
    public override void NodeComplete(string nextNode, System.Action onComplete)
    { }
    public override void OnLineStatusChanged(LocalizedLine dialogueLine)
    { }
    public override void RunLine(LocalizedLine dialogueLine, System.Action onDialogueLineFinished)
    {
        StartCoroutine(ShowLine(dialogueLine, onDialogueLineFinished, 50f));
    }
    private IEnumerator ShowLine(LocalizedLine dialogueLine, System.Action onDialogueLineFinished, float lettersPerSecond)
    {
        if (isDialoguePaused)
        {
            //MakeOptionBox("...", Color.gray, null, true);
            MakeNeutralOptionBox("UI_continue_dialogue");
        }
        while (isDialoguePaused)
            yield return null;

        

        MarkupParseResult markup = dialogueLine.Text;


        string characterName = dialogueLine.CharacterName;
        string text = dialogueLine.TextWithoutCharacterName.Text;

        


        GameObject box = Instantiate(linePrefab, this.transform);
        TextMeshProUGUI textMesh = box.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = text;


        CharacterSO character = characters.Find(c => c.name == characterName);
        box.GetComponentInChildren<Image>().color = character.color;

        // if is answer, sound is fired in KeyPressed
        if (isAnswer)
        {
            isAnswer = false;
        }
        else 
        {
            Debug.Log("speak");
            onCharacterSpeak.Invoke(character.soundEventPath);
            
        }

        bool isPlayerCharacter = (characterName == characters[0].name);
        if (!isPlayerCharacter)
        {
            // not me talking, invert line order
            box.transform.GetChild(0).SetAsLastSibling();
            box.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
        }
        
        if (character.pictures.Exists(p => p.name == pictureName))
        {
            character.SetPicture(pictureName);
        }
        else
        {
            pictureName = previousPictureName;
        }


        if (characterName != previousCharacterName)
        {
            // instantiate name bubble
            var bubble = Instantiate(nameBubblePrefab, box.transform);

            var rect = bubble.GetComponent<RectTransform>();
            rect.transform.GetChild(0).GetComponent<Image>().color = character.color;
            rect.GetComponentInChildren<TextMeshProUGUI>().text = character.name.ToUpper();
            if (!isPlayerCharacter)
            {
                rect.pivot = new Vector2(0, 0.5f);
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 1);
                rect.anchoredPosition = new Vector2(-rect.anchoredPosition.x, rect.anchoredPosition.y);
            }

        }

        bool drawIcon = (characterName != previousCharacterName || pictureName != previousPictureName);
        previousPictureName = pictureName;
        if (drawIcon)
        {
            // put in the character's picture
            var picture = Instantiate(picturePrefab, box.transform);
            character.SetPicture(pictureName);
            picture.GetComponent<Image>().sprite = character.Picture;
            if (!isPlayerCharacter)
            {
                var rect = picture.GetComponent<RectTransform>();
                rect.pivot = new Vector2(1, 1);
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 1);
                rect.anchoredPosition = new Vector2(-rect.anchoredPosition.x, rect.anchoredPosition.y);
            }
            

            
            previousCharacterName = characterName;
        }

        

        

        textMesh.ForceMeshUpdate();
        RectTransform boxTransform = box.GetComponent<RectTransform>();
        shownBoxes.Add(boxTransform);
        if (shownBoxes.Count > maxBoxes)
        {
            StartCoroutine(FadeUI(shownBoxes[0], 1.0f, 0.0f, boxFadeOutTime));
            shownBoxes.RemoveAt(0);
        }
        StartCoroutine(ScaleUI(boxTransform, Vector3.zero, Vector3.one, boxPopupTime));

        int characterCount = textMesh.textInfo.characterCount;
        textMesh.maxVisibleCharacters = 0;
        float baseSecondsPerLetter = lettersPerSecond <= 0 ? 0f : 1.0f / lettersPerSecond;
        float timeSinceLastCharacter = 0.0f;

        float secondsPerLetter = baseSecondsPerLetter;

        while (textMesh.maxVisibleCharacters < characterCount)
        {

            while (timeSinceLastCharacter >= secondsPerLetter && textMesh.maxVisibleCharacters < textMesh.textInfo.characterCount)
            {
                textMesh.maxVisibleCharacters += 1;
                timeSinceLastCharacter = 0.0f;
            }
            timeSinceLastCharacter += Time.deltaTime;
            yield return null;
        }

        isExpectingInput = true;
        isDialoguePaused = true;
        onDialogueLineFinished();

        yield return null;
    }

    public void ChangePicture(string name)
    {
        pictureName = name;
        Debug.Log("picture name : " + name);
    }

    public void UnlockFabric(string name)
    {
        newFabric = inputManager.fabrics.Find(f => f.name == name);
        onDialogueStop.AddListener(ShowNewFabric);
    }

    public void ShowNewFabric()
    {
        if (!availableFabrics.Contains(newFabric))
        {
            availableFabrics.Add(newFabric);
            // then show the popup
            GameObject unlockPopup = Instantiate(fabricUnlockPrefab, transform.parent);
            NewFabricFiller filler = unlockPopup.GetComponent<NewFabricFiller>();
            filler.Fill(newFabric);
            inputManager.SwitchTo(filler);
        }
        onDialogueStop.RemoveListener(ShowNewFabric);
    }

    public override void RunOptions(DialogueOption[] dialogueOptions, System.Action<int> onOptionSelected)
    {
        isDialoguePaused = false;
        options.Clear();
        optionBoxes.Clear();
        optionSelected = onOptionSelected;
        Debug.Log(dialogueOptions.Length + " options listed");



        foreach(var option in dialogueOptions)
        {
            string text = option.Line.Text.Text;


            MarkupAttribute tag = new MarkupAttribute();
            foreach (MarkupAttribute attribute in option.Line.Text.Attributes)
            {
                if (attribute.Name == "tag")
                    tag = attribute;
            }
            Color optionColor = Color.gray;
            if (tag.Properties.ContainsKey(tag.Name))
            {
                string tagString = tag.Properties[tag.Name].StringValue;
                FabricSO fabric = inputManager.GetFabric(tagString);

                if (!availableFabrics.Contains(fabric))
                    continue;

                options.Add(fabric.key, option);
                optionColor = fabric.color;
                GameObject optionbox = MakeOptionBox(text, optionColor, fabric.texture);
                optionBoxes.Add(fabric.key, optionbox);
            }
            
            

        }

    }

    private GameObject MakeOptionBox(string text, Color color, Sprite texture, bool selected = false)
    {
        GameObject optionBox = Instantiate(optionPrefab, optionsSelector.transform);
        TextMeshProUGUI textmesh = optionBox.GetComponentInChildren<TextMeshProUGUI>();

        OptionSetProperties properties = optionBox.GetComponent<OptionSetProperties>();
        properties.SetColor(color);
        properties.SetTexture(texture);
        properties.SetText(text);
        optionsSelector.transform.SetAsLastSibling();
        if (selected)
            optionBox.GetComponent<Animator>().SetBool("Selected", true);
        return optionBox;
    }

    private GameObject MakeNeutralOptionBox(string node = "")
    {
        if (availableFabrics.Count <= 0)
            return null;
        GameObject neutralOption = Instantiate(neutralOptionPrefab, optionsSelector.transform);
        NeutralOptionSetProperties properties = neutralOption.GetComponent<NeutralOptionSetProperties>();
        properties.SetSelectColor(Color.white);
        properties.CreateBands(availableFabrics);
        //properties.SetText(text);
        //properties.TextFromNode()

        UI_MenuFiller filler = menuRunner.GetComponent<UI_MenuFiller>();
        filler.textFields.Add(neutralOption.GetComponentInChildren<TMPro.TextMeshProUGUI>());
        menuRunner.StartDialogue(node);
        neutralOption.GetComponent<Animator>().SetBool("Selected", true);

        optionsSelector.transform.SetAsLastSibling();
        return neutralOption;
    }

    private IEnumerator ScaleUI(RectTransform what, Vector3 from, Vector3 to, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;
        while (Time.time <= endTime)
        {
            what.localScale = Vector3.Lerp(from, to, Mathf.InverseLerp(startTime, endTime, Time.time));
            yield return null;
        }
    }

    private IEnumerator FadeUI(RectTransform what, float from, float to, float duration)
    {
        CanvasGroup group = what.GetComponent<CanvasGroup>();
        if (group == null)
            yield break;
        float startTime = Time.time;
        float endTime = startTime + duration;
        while (Time.time <= endTime)
        {
            group.alpha = Mathf.Lerp(from, to, Mathf.InverseLerp(startTime, endTime, Time.time));
            yield return null;
        }
        group.alpha = to;
    }

    public void KeyPressed(FabricSO fabric)
    {
        if (!isExpectingInput)
            return;
        string k = fabric.key;
        if (isDialogueEnd)
        {
            ClearOptions();
            //gameObject.SetActive(false);
            foreach (Transform child in transform)
            {
                if (child.gameObject != optionsSelector)
                {
                    //child.parent = null;
                    Destroy(child.gameObject);
                }
            }
            onDialogueStop.Invoke();
            isDialogueEnd = false;
            isDialoguePaused = false;
            isExpectingInput = false;
            return;
        }
        if (isDialoguePaused)
        {
            isDialoguePaused = false;
            isExpectingInput = false;
            ClearOptions();
            return;
        }
        foreach (string key in options.Keys)
        {
            k = k.ToUpper();
            if (key == k)
            {
                // first press selects
                if (selectedKey != k)
                {
                    if (selectedKey != "")
                        optionBoxes[selectedKey].GetComponent<Animator>().SetBool("Selected", false);
                    selectedKey = k;
                    optionBoxes[selectedKey].GetComponent<Animator>().SetBool("Selected", true);
                    onPreSelect.Invoke(FabricFromKey(k).soundIndex);
                    break;
                }
                else
                {
                    onAnswer.Invoke(FabricFromKey(k).soundIndex);
                    isAnswer = true;
                    optionSelected(options[key].DialogueOptionID);
                    selectedKey = "";
                    isExpectingInput = false;
                    ClearOptions();

                    
                    break;
                }
            }
        }
        
    }

    private void ClearOptions()
    {
        foreach(Transform t in optionsSelector.transform)
        {
            Destroy(t.gameObject);
        }
    }

    public void MaxAnalogValue(string k, int value)
    {

    }

    private FabricSO FabricFromKey(string k)
    {
        return availableFabrics.Find(f => f.key == k);
    }

    // Start is called before the first frame update
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        //DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
        dialogueRunner.AddCommandHandler("picture", (string name) => ChangePicture(name));
        dialogueRunner.AddCommandHandler("unlock", (string name) => UnlockFabric(name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
