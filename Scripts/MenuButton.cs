using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public FabricSO fabric;
    public Animator animator;

    public UnityEvent OnSelect;
    public UnityEvent OnPress;
    public void SetSelected(bool state)
    {
        animator.SetBool("Selected", state);
        OnSelect.Invoke();
    }

    public void Press()
    {
        OnPress.Invoke();
        
    }

    public void ChangeScene(string sceneName)
    {
        float duration = 1.0f;
        GameObject.Find("Fondu Noir").GetComponent<UI_Fondu>().FadeAlpha(0, 1.1f, duration);
        StartCoroutine(DoChangeScene(sceneName, duration));
    }

    private IEnumerator DoChangeScene(string sceneName, float minDuration)
    {
        var operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(minDuration);
        operation.allowSceneActivation = true;
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
