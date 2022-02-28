using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCameraPosition : MonoBehaviour
{
    public Transform mainCharacter;
    public Transform otherCharacter;
    public float distance = 5.0f;
    public float height = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!mainCharacter || !otherCharacter)
            return;
        Vector3 middle = (mainCharacter.position + otherCharacter.position) / 2.0f;
        Debug.DrawLine(middle, middle + Vector3.up);
        transform.position = middle;
        transform.LookAt(otherCharacter, Vector3.up);
        transform.Rotate(0, 90, 0);
        transform.Translate(transform.forward * -distance, Space.World);
        transform.Translate(Vector3.up * height, Space.World);
    }
}
