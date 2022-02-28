using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFlipTrigger : MonoBehaviour
{
    public float interval = 0.2f;
    public List<TileSwitcher> switchers;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(FlipTiles(interval));
    }

    private IEnumerator FlipTiles(float interval)
    {
        foreach(var s in switchers)
        {
            yield return new WaitForSeconds(interval);
            s.Flip();
        }
        gameObject.SetActive(false);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
