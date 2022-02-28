using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class SceneSwitch : CutsceneBehaviour
{
    public string sceneToLoad;
    public UI_Fondu fondu;
    public float duration = 1.0f;
    public float waitBeforeSwitch = 1.7f;
    public float waitAfterDark = 1.0f;
    public bool stopStounds = true;
    public override void Play()
    {
        fondu.gameObject.SetActive(true);
        fondu.GetComponent<CanvasGroup>().alpha = 0.0f;
        StartCoroutine(DoChangeScene(sceneToLoad, duration));
    }

    private IEnumerator DoChangeScene(string sceneName, float minDuration)
    {
        yield return new WaitForSeconds(waitBeforeSwitch);
        fondu.FadeAlpha(0, 1.1f, duration);
        var operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(minDuration);
        yield return new WaitForSeconds(waitAfterDark);
        if (stopStounds)
            RuntimeManager.GetBus("bus:/").stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
