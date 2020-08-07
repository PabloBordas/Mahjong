using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public int row;
    public int column;
    public int tileID = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        transform.parent.GetComponent<GameController>().recentlyClicked = gameObject;
        transform.parent.GetComponent<GameController>().CheckClick();
    }
}
