using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Patchwork/Fabric Type")]
public class FabricSO : ScriptableObject
{
    public string key;
    public string tag;
    public Color color;
    public Sprite texture;
    public int ficelle;
    public int soundIndex;
}
