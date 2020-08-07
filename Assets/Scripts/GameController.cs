using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject boardController;
    public GameObject firstClicked;
    public GameObject secondClicked;
    public GameObject thirdClicked;
    public GameObject recentlyClicked;
    public GameObject lastClicked;
    public bool check;
    public int botRow;
    public int topRow;
    public int botColumn;
    public int topColumn;
    public int Score;
    public List<GameObject[]> boardTiles = new List<GameObject[]>();
    public TextMeshProUGUI ScoreText;
    // Start is called before the first frame update
    void Start()
    {
        boardTiles = boardController.GetComponent<BoardController>().boardTiles;
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.SetText("Score " +  Score);
    }

    public void CheckClick(){
        if(firstClicked==null && recentlyClicked.GetComponent<TileController>().tileID==0)
        {
            Debug.Log("Wrong first Click");
        }
        if(firstClicked==null && recentlyClicked.GetComponent<TileController>().tileID!=0)
        {
            firstClicked = recentlyClicked;
            Debug.Log("First Click");
        }
        else if(firstClicked==recentlyClicked)
        {
            firstClicked = null;
            Debug.Log("Remove 1 Click");
        }
        else if(secondClicked==recentlyClicked)
        {
            secondClicked = null;
            Debug.Log("Remove 2 Click");
        }
        else if(thirdClicked==recentlyClicked)
        {
            thirdClicked = null;
            Debug.Log("Remove 3 Click");
        }
        else if(firstClicked!=null && secondClicked!=null && thirdClicked!=null && firstClicked!=recentlyClicked && secondClicked!=recentlyClicked && thirdClicked!=recentlyClicked){
            Debug.Log("more than 3 moves -10");
            if(Score-10>0){
               Score=-10; 
            }
            ResetClicks();
        }
        else if(firstClicked!=null && secondClicked==null)
        {
            if(recentlyClicked.GetComponent<TileController>().row!=firstClicked.GetComponent<TileController>().row 
                && recentlyClicked.GetComponent<TileController>().column!=firstClicked.GetComponent<TileController>().column){
                    Debug.Log("Wrong second move -10");
                    if(Score-10>0){
                    Score=-10; 
                    }
                    ResetClicks();
            }else if(recentlyClicked.GetComponent<TileController>().tileID==0){        
                secondClicked = recentlyClicked;
                Debug.Log("second Click");
            }else{
                secondClicked = recentlyClicked;
                Debug.Log("second Click to check");
                CheckMovement(false);
                ResetClicks();
            }
        }
        else if(firstClicked!=null && secondClicked!=null && thirdClicked==null)
        {
            if(recentlyClicked.GetComponent<TileController>().row!=secondClicked.GetComponent<TileController>().row 
                && recentlyClicked.GetComponent<TileController>().column!=secondClicked.GetComponent<TileController>().column){
                    Debug.Log("Wrong third move -10");
                    if(Score-10>0){
                    Score=-10; 
                    }
                    ResetClicks();
            }else if(recentlyClicked.GetComponent<TileController>().tileID!=0){        
                thirdClicked = recentlyClicked;
                Debug.Log("third Click");
                CheckMovement(true);
                ResetClicks();
            }
            
        }
        lastClicked = recentlyClicked;
    }

    public void CheckMovement(bool moreThan2Clicks){
        bool wrongMove = false;
        if(!moreThan2Clicks){
            if(firstClicked.GetComponent<TileController>().row==secondClicked.GetComponent<TileController>().row){
                GameObject[] iArrayGO = boardTiles[firstClicked.GetComponent<TileController>().row];
                for(int i = 0; i < iArrayGO.Length;i++){
                    if((i < firstClicked.GetComponent<TileController>().row && i > secondClicked.GetComponent<TileController>().row) ||
                     (i > firstClicked.GetComponent<TileController>().row && i < secondClicked.GetComponent<TileController>().row)){
                        if(iArrayGO[i].GetComponent<TileController>().tileID!=0){
                            Debug.Log("Wrong second move -10");
                            wrongMove=true;
                            if(Score-10>0){
                            Score=-10; 
                            }
                            break;
                        }
                    }
                }
            }else{
                int i = 0;
                foreach(GameObject[] iArrayGO in boardTiles){
                    if((i < firstClicked.GetComponent<TileController>().column && i > secondClicked.GetComponent<TileController>().column) ||
                     (i > firstClicked.GetComponent<TileController>().column && i < secondClicked.GetComponent<TileController>().column)){
                        if(iArrayGO[i].GetComponent<TileController>().tileID!=0){
                            Debug.Log("Wrong second move -10");
                            wrongMove = true;
                            if(Score-10>0){
                            Score=-10; 
                            }
                            break;
                        }
                    }
                    i++;
                }
            }
            if(firstClicked.GetComponent<TileController>().tileID == secondClicked.GetComponent<TileController>().tileID && !wrongMove){   
                Debug.Log("Nice move +15");   
                Score=+15;           
                firstClicked.GetComponent<SpriteRenderer>().sprite = null;
                secondClicked.GetComponent<SpriteRenderer>().sprite = null;
            }else{
                Debug.Log("Wrong second move -10");
                if(Score-10>0){
                Score=-10; 
                }
            }
        //3 clicks
        }else{
            if(firstClicked.GetComponent<TileController>().row==secondClicked.GetComponent<TileController>().row){
                Debug.Log("3 clicks 1-2 row");
                GameObject[] iArrayGO = boardTiles[firstClicked.GetComponent<TileController>().row];
                for(int i = 0; i < iArrayGO.Length;i++){
                    if((i <= firstClicked.GetComponent<TileController>().row && i >= secondClicked.GetComponent<TileController>().row) ||
                     (i >= firstClicked.GetComponent<TileController>().row && i <= secondClicked.GetComponent<TileController>().row) &&
                     iArrayGO[i] != firstClicked && iArrayGO[i] != secondClicked){
                        if(iArrayGO[i].GetComponent<TileController>().tileID!=0){
                            Debug.Log("Wrong third move -10");
                            wrongMove = true;
                            if(Score-10>0){
                            Score=-10; 
                            }
                            break;
                        }
                    }
                }
            }else{
                Debug.Log("3 clicks 1-2 colum");
                int i = 0;
                foreach(GameObject[] iArrayGO in boardTiles){
                    if((i <= firstClicked.GetComponent<TileController>().column && i >= secondClicked.GetComponent<TileController>().column) ||
                     (i >= firstClicked.GetComponent<TileController>().column && i <= secondClicked.GetComponent<TileController>().column) &&
                     iArrayGO[i] != firstClicked && iArrayGO[i] != secondClicked){
                        if(iArrayGO[i].GetComponent<TileController>().tileID!=0){
                            Debug.Log("Wrong third move -10");
                            wrongMove = true;
                            if(Score-10>0){
                            Score=-10; 
                            }
                            break;
                        }
                    }
                    i++;
                }
            }

            if(secondClicked.GetComponent<TileController>().row==thirdClicked.GetComponent<TileController>().row){
                Debug.Log("3 clicks 2-3 row");
                GameObject[] iArrayGO = boardTiles[firstClicked.GetComponent<TileController>().row];
                for(int i = 0; i < iArrayGO.Length;i++){
                    if((i <= secondClicked.GetComponent<TileController>().row && i >= thirdClicked.GetComponent<TileController>().row) ||
                     (i >= secondClicked.GetComponent<TileController>().row && i <= thirdClicked.GetComponent<TileController>().row) &&
                     iArrayGO[i] != thirdClicked && iArrayGO[i] != secondClicked){
                        if(iArrayGO[i].GetComponent<TileController>().tileID!=0){
                            Debug.Log("Wrong third move -10");
                            wrongMove = true;
                            if(Score-10>0){
                            Score=-10; 
                            }
                            break;
                        }
                    }
                }
            }else{
                Debug.Log("3 clicks 2-3 column");
                int i = 0;
                foreach(GameObject[] iArrayGO in boardTiles){
                    if((i <= secondClicked.GetComponent<TileController>().column && i >= thirdClicked.GetComponent<TileController>().column) ||
                     (i >= secondClicked.GetComponent<TileController>().column && i <= thirdClicked.GetComponent<TileController>().column) &&
                     iArrayGO[i] != thirdClicked && iArrayGO[i] != secondClicked){
                        if(iArrayGO[i].GetComponent<TileController>().tileID!=0){
                            Debug.Log("Wrong third move -10");
                            wrongMove = true;
                            if(Score-10>0){
                            Score=-10; 
                            }
                            break;
                        }
                    }
                    i++;
                }
            }

            if(firstClicked.GetComponent<TileController>().tileID == thirdClicked.GetComponent<TileController>().tileID && !wrongMove){   
                Debug.Log("Nice move +15");  
                Score=+15;           
                firstClicked.GetComponent<SpriteRenderer>().sprite = null;
                thirdClicked.GetComponent<SpriteRenderer>().sprite = null;
            }else{
                Debug.Log("Wrong third move -10");
                if(Score-10>0){
                Score=-10; 
                }
            }
        }

    }

    public void ResetClicks(){
        firstClicked = null;
        secondClicked = null;
        thirdClicked = null;
        recentlyClicked = null;
        lastClicked = null;
        botRow = -1;
        topRow = -1;
        botColumn = -1;
        topColumn = -1;
    }
}
