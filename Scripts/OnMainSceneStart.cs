using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMainSceneStart : MonoBehaviour
{
    public UI_Fondu fondu;

    private void OnTriggerEnter(Collider other)
    {
        fondu.FadeAlpha(1, 0, 1);
        GetComponent<TriggerCutscene>().Play();
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
