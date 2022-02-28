using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class NeutralOptionSetProperties : MonoBehaviour
{
    public Transform bar1;
    public Transform bar2;
    public Image selectionImage;
    public Image coverImage;
    public GameObject bandPrefab;
    public TMPro.TextMeshProUGUI textMesh;
    

    public void CreateBands(IEnumerable<FabricSO> availableFabrics)
    {
        foreach (FabricSO fabric in availableFabrics)
        {
            GameObject band = Instantiate(bandPrefab, bar1);
            Image bandTexture = band.transform.GetChild(0).GetComponent<Image>();
            bandTexture.sprite = fabric.texture;
            GameObject band2 = Instantiate(band, bar2);

        }
    }

    public void SetCoverColor(Color color)
    {
        coverImage.color = color;
    }
    public void SetSelectColor(Color color)
    {
        selectionImage.color = color;
    }
    public void SetText(string text)
    {
        textMesh.text = text;
    }
    
    

}
