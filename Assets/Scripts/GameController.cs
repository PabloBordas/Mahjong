using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class GameController : MonoBehaviour
{
    public GameObject boardController;
    public GameObject TitleController;
    public GameObject firstClicked;
    public GameObject secondClicked;
    public GameObject recentlyClicked;
    public bool check;
    public bool soundIsOn;
    public Vector2 firstLocation = new Vector2();
    public Vector2 secondLocation = new Vector2();
    public int firstTileId;
    public int secondTileId;
    public int score;
    private int lastTopScore;
    public int[] topScores;
    public MenuController MenuControllerScript;
    public List<GameObject[]> boardTiles = new List<GameObject[]>();
    public GameObject[][] boardGridTiles = new GameObject[20][];
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI ComboText;
    public TextMeshProUGUI newPointsText;
    public GameObject wrongMoveText;
    public int tilesRemaining = 0;
    private int lvlLoaded;
    public AudioSource audioSource;
    public AudioClip mistakeSound;
    public int combo;
    // Start is called before the first frame update
    void Start()
    {
        Sound();
        Time.timeScale = 1f;
        audioSource = GetComponent<AudioSource>();
        if(soundIsOn){
            audioSource.volume = 0.10f;
        }else{
            audioSource.volume = 0f;
        }
        combo = 1;
        ComboText.SetText("");
        newPointsText.SetText("");
        lvlLoaded = boardController.GetComponent<BoardController>().lvlLoaded-1;
        tilesRemaining = boardController.GetComponent<BoardController>().totalTiles; 
        boardTiles = boardController.GetComponent<BoardController>().boardTiles;
        boardGridTiles = boardController.GetComponent<BoardController>().boardGridTiles;
        score = 0;
    }

    public void CheckClick(){
        if(firstClicked==null && recentlyClicked.GetComponent<TileController>().tileID==0)
        {
            //first click on blank tile, we dont check nothing
            Debug.Log("Wrong first Click - Blank Tile");
        }
        else if(firstClicked==null && recentlyClicked.GetComponent<TileController>().tileID!=0)
        {
            //first click on tile filled, set up variables and wait for the second click
            firstClicked = recentlyClicked;
            //set child sprite color to blue to give user feedback
            firstClicked.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
            firstTileId = firstClicked.GetComponent<TileController>().tileID;
            firstLocation = firstClicked.GetComponent<TileController>().location;
            Debug.Log("First Click done");
        }
        else if(firstClicked==recentlyClicked)
        {
            //Player click on the same tile to remove first tile selected
            Debug.Log("Remove 1 Click");
            ResetClicks();
        }
        else if(firstClicked!=null && secondClicked==null && recentlyClicked.GetComponent<TileController>().tileID!=0)
        {            
            Debug.Log("Second Click done");            
            secondClicked = recentlyClicked;
            //set child sprite color to blue to give user feedback
            secondClicked.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
            secondTileId = secondClicked.GetComponent<TileController>().tileID;
            secondLocation = secondClicked.GetComponent<TileController>().location;
            
            if(firstTileId!=secondTileId)
            {
                StartCoroutine("NotifyError");
                IncreaseScore(false);
            }
            else
            {
                if(((firstLocation.x+1 == secondLocation.x || firstLocation.x-1 == secondLocation.x) && firstLocation.y== secondLocation.y) 
                || ((firstLocation.y+1 == secondLocation.y || firstLocation.y-1 == secondLocation.y) && firstLocation.x== secondLocation.x)){
                    Debug.Log("Neighbour");
                    IncreaseScore(true);
                    //ResetClicks();
                }else{
                    CheckMovement();
                }
            }
            
            
        }
        
    }

    public void CheckMovement(){

        List<GameObject> avaliableFirstSearch = SearchAvaliable(firstLocation);
        List<GameObject> avaliableSecondSearch = new List<GameObject>();

        foreach(GameObject iAvaliable in avaliableFirstSearch){
            avaliableSecondSearch.AddRange(SearchAvaliable(iAvaliable.GetComponent<TileController>().location));
        }
        
        List<GameObject> avaliableFirstClick = new List<GameObject>();
        avaliableFirstClick.AddRange(avaliableFirstSearch);
        avaliableFirstClick.AddRange(avaliableSecondSearch);
        avaliableFirstClick.Add(firstClicked);          

        List<GameObject> avaliableSecondClick = SearchAvaliable(secondLocation);

        bool niceMove = false;
        foreach(GameObject iGO in avaliableSecondClick){
            if(avaliableFirstClick.Contains(iGO)){
                niceMove = true;
                break;
            }
        }
        if(!niceMove){
            StartCoroutine("NotifyError");
        }

        IncreaseScore(niceMove);
    }

    public List<GameObject> SearchAvaliable(Vector2 locationToCheck){

        List<GameObject> result = new List<GameObject>();
        Vector2 limits = new Vector2(boardController.GetComponent<BoardController>().maxX,boardController.GetComponent<BoardController>().maxY);
        int checkingX = (int) locationToCheck.x;
        int checkingY = (int) locationToCheck.y;
        
        //check right side
        while((checkingX+1)<limits.x){
            checkingX++;
            if(boardGridTiles[checkingY][checkingX].GetComponent<TileController>().tileID==0){
                result.Add(boardGridTiles[checkingY][checkingX]);  
            }else{
                break;
            }        
        }
        checkingX = (int) locationToCheck.x;

        //check left side
        while((checkingX-1)>=0){
            checkingX--;
            if(boardGridTiles[checkingY][checkingX].GetComponent<TileController>().tileID==0){
                result.Add(boardGridTiles[checkingY][checkingX]);  
            }else{
                break;
            } 
        }
        checkingX = (int) locationToCheck.x;

        //check top side
        while((checkingY+1)<limits.y){
            checkingY++;
            if(boardGridTiles[checkingY][checkingX].GetComponent<TileController>().tileID==0){
                result.Add(boardGridTiles[checkingY][checkingX]);  
            }else{
                break;
            }        
        }
        checkingY = (int) locationToCheck.y;
        
        //check bot side
        while((checkingY-1)>=0){
            checkingY--;
            if(boardGridTiles[checkingY][checkingX].GetComponent<TileController>().tileID==0){
                result.Add(boardGridTiles[checkingY][checkingX]);  
            }else{
                break;
            } 
        }

        return result;
    }

    public void IncreaseScore(bool addPoints){
        //IEnumerator newPointsCorutine;
        if(!addPoints){
            Debug.Log("Wrong move -10");
            if(score-10>=0){
                score-=10;
                StartCoroutine(NotifyNewPoints(-10,Color.red));
            }else{
                score = 0;
                StartCoroutine(NotifyNewPoints(0,Color.red));
            }
            combo = 1;
            ComboText.SetText("");
            ResetClicks();
        }else{
            firstClicked.GetComponent<SpriteRenderer>().sprite = null;
            firstClicked.GetComponent<TileController>().tileID = 0;
            secondClicked.GetComponent<SpriteRenderer>().sprite = null; 
            secondClicked.GetComponent<TileController>().tileID = 0;
            tilesRemaining = tilesRemaining-2;
            Debug.Log("Nice move +15");
            int pointsToAdd = 15*combo;
            if(combo<5){
                StartCoroutine(NotifyNewPoints(pointsToAdd,Color.green));
            }else if(combo>=5 && combo<=10){
                StartCoroutine(NotifyNewPoints(pointsToAdd,Color.cyan));
            }else if(combo>10){
                StartCoroutine(NotifyNewPoints(pointsToAdd,Color.yellow));
            }
            score+=pointsToAdd; 
            combo++;
            ComboText.SetText("X"+combo);
            if(tilesRemaining <= 0){
                LevelEnd(true);
                ResetClicks();
            }
            else
            {
                ResetClicks();
                bool movesAvaliables = CheckHint(false);
                if(!movesAvaliables){
                    LevelEnd(false);
                }
            }
        }
        ScoreText.SetText("Score " +  score);
    }

    public void ResetClicks(){
        Debug.Log("Reseting clicks");
        //setting back the background color of the tile selected
        if(firstClicked!=null){
            if(firstClicked.GetComponent<TileController>().tileID!=0){
                firstClicked.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            }else{
                firstClicked.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,.1f);
            }
            firstClicked = null;
            firstLocation = new Vector2();
        }
        if(secondClicked!=null){
            if(secondClicked.GetComponent<TileController>().tileID!=0){
                secondClicked.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            }else{
                secondClicked.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,.1f);
            }
            secondClicked = null;
            secondLocation = new Vector2();
        }
        recentlyClicked = null;
    }

    public void LevelEnd(bool success){
        Debug.Log("Game ended! win? " + success);
        if(success){
            SaveData(true);
            MenuControllerScript.Win(score, lastTopScore);    
        }else{
            MenuControllerScript.Lose();
        }
    }

    public void CheckHintUser(){
        CheckHint(true);
    }
    public bool CheckHint(bool userNeedHelp){
        bool avaliablePair = false;
        List<GameObject> avaliableTiles = new List<GameObject>();

        for(int i = 0; i<boardGridTiles.Length; i++){
            if(boardGridTiles[i]!=null){
                for(int j = 0; j<boardGridTiles[i].Length; j++){

                    if(boardGridTiles[i][j]==null){
                        break;
                    }
                    if(boardGridTiles[i][j].GetComponent<TileController>().tileID!=0){
                        avaliableTiles.Add(boardGridTiles[i][j]);  
                    }
                } 
            }else{
                break;
            }
        } 

        for(int i = 0; i<avaliableTiles.Count; i++){
            
            firstClicked = avaliableTiles[i];
            firstTileId = firstClicked.GetComponent<TileController>().tileID;
            firstLocation = firstClicked.GetComponent<TileController>().location;
            for(int j = 0; j<avaliableTiles.Count; j++){
                secondClicked = avaliableTiles[j];
                secondTileId = secondClicked.GetComponent<TileController>().tileID;
                secondLocation = secondClicked.GetComponent<TileController>().location;      
                if(firstTileId==secondTileId && firstClicked!=secondClicked)
                {
                    if(((firstLocation.x+1 == secondLocation.x || firstLocation.x-1 == secondLocation.x) && firstLocation.y== secondLocation.y) 
                    || ((firstLocation.y+1 == secondLocation.y || firstLocation.y-1 == secondLocation.y) && firstLocation.x== secondLocation.x)){
                        avaliablePair = true;
                        break;
                    }else{
                        List<GameObject> avaliableFirstSearch = SearchAvaliable(firstLocation);
                        List<GameObject> avaliableSecondSearch = new List<GameObject>();

                        foreach(GameObject iAvaliable in avaliableFirstSearch){
                            avaliableSecondSearch.AddRange(SearchAvaliable(iAvaliable.GetComponent<TileController>().location));
                        }
                        
                        List<GameObject> avaliableFirstClick = new List<GameObject>();
                        avaliableFirstClick.AddRange(avaliableFirstSearch);
                        avaliableFirstClick.AddRange(avaliableSecondSearch);
                        avaliableFirstClick.Add(firstClicked);          

                        List<GameObject> avaliableSecondClick = SearchAvaliable(secondLocation);

                        foreach(GameObject iGO in avaliableSecondClick){
                            if(avaliableFirstClick.Contains(iGO)){
                                avaliablePair = true;
                                break;
                            }
                        }
                        if(avaliablePair){
                            break;
                        }
                    }
                }     
            }//end for 
            if(avaliablePair){
                    break;
            } 
        }//end for

        if(avaliablePair && userNeedHelp){
            IEnumerator coroutineCH;
            coroutineCH = ColorHint(firstClicked,secondClicked);
            StartCoroutine(coroutineCH);
        }else{
            ResetClicks();
        }

        
        Debug.Log("Exist avaliable movements?  " + avaliablePair);
        return avaliablePair;
    }

    public IEnumerator ColorHint(GameObject firstHint, GameObject secondHint){
        Debug.Log("ColorHint firstHint.name: " + firstHint.transform.name);
        Debug.Log("ColorHint secondHint.name: " + secondHint.transform.name);
        firstHint.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
        secondHint.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(1f);
        firstHint.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        secondHint.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        ResetClicks();      
    }

    public IEnumerator NotifyError(){
        if(soundIsOn){
            audioSource.PlayOneShot(mistakeSound, MenuControllerScript.volumen);
        }        
        wrongMoveText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        wrongMoveText.SetActive(false);
    }

    public IEnumerator NotifyNewPoints(int points, Color color){
        
        if(points<=0){
            newPointsText.SetText(""+points); 
        }else{
            newPointsText.SetText("+"+points); 
        }
        
        newPointsText.color = color;       
        yield return new WaitForSeconds(0.5f); 
        newPointsText.SetText(""); 
    }

    public void SaveData(bool lvlCompleted){
        PlayerData playerData = SaveSystem.LoadPlayer();        
        int numberOfLevels = boardController.GetComponent<BoardController>().numberOfLevels;        
        if(playerData!=null && playerData.topScores.Length>0){
            topScores = playerData.topScores;  
            lastTopScore = topScores[lvlLoaded];
            if(topScores[lvlLoaded]<score && lvlCompleted){
                topScores[lvlLoaded]=score;
            }         
        }else{
            topScores = new int[numberOfLevels];
            for(int i = 0; i<numberOfLevels; i++){
                if(i==lvlLoaded && lvlCompleted){
                    topScores[lvlLoaded]=score;
                }else{
                    topScores[i]=0;
                }
            }
        }
        
        SaveSystem.SaveScores(this);
    }

    public void Sound(){
        
        PlayerData playerData = SaveSystem.LoadPlayer();
        Debug.Log("Loading Sound... ");   
        if(playerData!=null){
            soundIsOn = playerData.soundIsOn;
        }else{
            soundIsOn = true;
        }
    
    }

}
