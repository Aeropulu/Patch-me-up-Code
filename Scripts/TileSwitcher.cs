using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TileSwitcher : MonoBehaviour
{
    [SerializeField]
    private int tile_index;
    [SerializeField]
    private GameObject[] tiles;

    private PlayableDirector director;

    private TrackAsset flipUpTrack;
    private TrackAsset activateUpTrack;
    private TrackAsset flipDownTrack;
    private TrackAsset activateDownTrack;

    FMOD.Studio.EventInstance SFX_Objects_Tiles_Rotate;

    public int TileIndex
    { 
        get => tile_index;
        private set
        {
            SwitchTo(value);
        }
    }


    public void SwitchTo(int index)
    {
        int old_index = tile_index;
        tile_index = index;
        if (tile_index == old_index)
            return;

        if (tile_index < 0 || tile_index >= tiles.Length)
            return;

        Debug.Log("coin " + flipUpTrack.name + flipDownTrack.name);
        
        

        director.SetGenericBinding(flipUpTrack, tiles[tile_index].GetComponent<Animator>());
        director.SetGenericBinding(activateUpTrack, tiles[tile_index]);
        director.SetGenericBinding(flipDownTrack, tiles[old_index].GetComponent<Animator>());
        director.SetGenericBinding(activateDownTrack, tiles[old_index]);
        director.Play();

    }

    public void Flip()
    {
        SFX_Objects_Tiles_Rotate = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_Objects/SFX_Objects_Tiles_Rotate");
        SFX_Objects_Tiles_Rotate.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        SFX_Objects_Tiles_Rotate.start();
        int i = tile_index + 1;
        if (i >= tiles.Length)
            i = 0;
        SwitchTo(i);
    }
    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();

        TimelineAsset timelineAsset = (TimelineAsset)director.playableAsset;

        flipUpTrack = timelineAsset.GetOutputTrack(1); // Tile Flip Up
        activateUpTrack = timelineAsset.GetOutputTrack(2); // Tile Up Activate
        flipDownTrack = timelineAsset.GetOutputTrack(3); // Tile Flip Down
        activateDownTrack = timelineAsset.GetOutputTrack(4); // Tile Down Activate

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
