using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CutsceneBehaviour : MonoBehaviour
{
    public UnityEvent OnComplete;
    public abstract void Play();
}
