using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int[] topScores;
    public bool soundIsOn;
    public PlayerData(GameController gameController){
        topScores = gameController.topScores;
        soundIsOn = gameController.soundIsOn;
    }
}
