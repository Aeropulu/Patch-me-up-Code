using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareFader : MonoBehaviour
{
    private MaterialPropertyBlock block;
    private Renderer[] renderers;

    public float alpha = 1.0f;
    private float old_alpha;
    
    
    // Start is called before the first frame update
    void Start()
    {
        old_alpha = alpha;
        renderers = GetComponentsInChildren<Renderer>();
        block = new MaterialPropertyBlock();
        UpdateAlpha();
    }

    // Update is called once per frame
    void Update()
    {
        if (alpha != old_alpha)
        {
            UpdateAlpha();
            
            old_alpha = alpha;
        }
    }

    private void UpdateAlpha()
    {
        foreach (Renderer r in renderers)
        {
            string name;
            if (r.GetComponent<SquareInput>()) // is text
                name = "_FaceColor";
            else // is square
                name = "_BaseColor";
            Color col = r.material.GetColor(name);
            block.SetColor(name, new Color(col.r, col.g, col.b, alpha));
            r.SetPropertyBlock(block);
        }
    }

    
}
