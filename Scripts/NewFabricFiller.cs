using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using UnityEngine.Events;

public class NewFabricFiller : MonoBehaviour, IInputReceiver
{
    public string node;
    public List<TMPro.TextMeshProUGUI> textFields;
    public Image fabricImage;
    public Image sparks;
    public TMPro.TextMeshProUGUI numberText;
    public UnityEvent onAppear;
    public UnityEvent<int> onConfirmed;

    private FabricSO thisFabric;

    public void Fill(FabricSO fabric)
    {
        DialogueRunner runner = GameObject.Find("Menu UI Runner").GetComponent<DialogueRunner>();
        UI_MenuFiller filler = runner.GetComponent<UI_MenuFiller>();
        foreach (var f in textFields)
        {
            filler.textFields.Add(f);
        }
        runner.StartDialogue(node);
        fabricImage.sprite = fabric.texture;
        sparks.color = fabric.color;
        numberText.text = fabric.ficelle.ToString();
        thisFabric = fabric;
        StartCoroutine(FadeScale(0.1f, 1, 0.2f));
        onAppear.Invoke();
    }

    public void KeyPressed(FabricSO fabric)
    {
        if(fabric.key == thisFabric.key)
        {
            FindObjectOfType<InputManager>().SwitchToMovement();
            onConfirmed.Invoke(thisFabric.soundIndex);
            StartCoroutine(FadeScale(1, 0.1f, 0.2f));
            Destroy(gameObject, 0.21f);
        }
    }

    private IEnumerator FadeScale(float from, float to, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;
        RectTransform rect = GetComponent<RectTransform>();
        while (Time.time <= endTime)
        {
            rect.localScale = Vector3.one * Mathf.Lerp(from, to, Mathf.InverseLerp(startTime, endTime, Time.time));
            yield return null;
        }
        rect.localScale = Vector3.one * to;
    }
    public void MaxAnalogValue(string k, int value)
    {
        throw new System.NotImplementedException();
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
