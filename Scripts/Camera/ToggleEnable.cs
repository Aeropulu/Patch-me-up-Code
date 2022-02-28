using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
public class ToggleEnable : MonoBehaviour
{
    [SerializeField]
    private KeyCode key = KeyCode.Return;
    private Cinemachine.CinemachineVirtualCamera cam;
    void Start()
    {
        cam = GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            cam.enabled = !cam.enabled;
        }
    }
}
