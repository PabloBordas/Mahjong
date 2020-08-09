using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public int tileID = 0;
    public Vector2 location = new Vector2();

    void OnMouseDown()
    {
        if(!MenuController.GameIsPaused){
            transform.parent.GetComponent<GameController>().recentlyClicked = gameObject;
            transform.parent.GetComponent<GameController>().CheckClick();
        }
    }
}
