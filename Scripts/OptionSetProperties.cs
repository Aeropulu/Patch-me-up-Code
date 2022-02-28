using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSetProperties : MonoBehaviour
{
    public Image selectionImage;
    public Image textureImage;
    public TMPro.TextMeshProUGUI textMesh;
    
    public void SetColor(Color color)
    {
        selectionImage.color = color;
    }

    public void SetTexture(Sprite texture)
    {
        if (texture == null)
            return;
        textureImage.sprite = texture;
    }

    public void SetText(string text)
    {
        textMesh.text = text;
        textMesh.ForceMeshUpdate();
    }
}
