using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="Patchwork/Character")]
public class CharacterSO : ScriptableObject
{
    public Sprite Picture
    {
        get => pictures[currentPictureIndex];
    }
    public List<Sprite> pictures;
    public Color color;
    [FMODUnity.EventRef]
    public string soundEventPath;
    private int currentPictureIndex = 0;


    public void SetPicture(string name)
    {
        Sprite newpic = pictures.Find(s => s.name == name);
        if (newpic == null)
            return;
        currentPictureIndex = pictures.IndexOf(newpic);
    }
    private void Awake()
    {
        currentPictureIndex = 0;
    }
}
