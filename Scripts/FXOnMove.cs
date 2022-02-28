using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXOnMove : MonoBehaviour
{
    public GameObject FXPrefab;
    [SerializeField]
    private float duration = 3.0f;
    [SerializeField]
    private float offset = 0.4f;
    public void SpawnFxAt(Vector3 position)
    {
        if (!FXPrefab) return;

        position += Vector3.up * offset;
        GameObject fx = Instantiate(FXPrefab, position, Quaternion.identity);
        Destroy(fx, duration);
    }
}
