using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class BoardController : MonoBehaviour
{
    public int lvlLoaded;
    public int numberOfLevels;
    public string pathLvl1 = "Assets/TestLayouts/11x11 [abstract].txt";
    public string pathLvl2 = "Assets/TestLayouts/11X12 - [up].txt";
    public string pathLvl3 = "Assets/TestLayouts/12X12 - [#].txt";
    public string pathLvl4 = "Assets/TestLayouts/13X11 - [1].txt";
    public string pathLvl5 = "Assets/TestLayouts/13x14 - [caro].txt";
    public List<char[]> board = new List<char[]>();
    public List<GameObject[]> boardTiles = new List<GameObject[]>();
    public GameObject[][] boardGridTiles = new GameObject[20][];
    public GameObject tilePrefab;
    public int totalTiles = 0;
    public int maxX = 0;
    public int maxY = 0;
    public List<int[]> listSprites = new List<int[]>();
    public string[] sets;
    public Sprite[] Sprites;
    public GameObject gameController;
    private List<Sprite> setSprites = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    { 
        lvlLoaded = TitleController.lvlLoaded;
        LoadLevelBoard();
        LoadSprites();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevelBoard(){

        switch(lvlLoaded)
        {
            case 1:
                using (StreamReader reader = new StreamReader(pathLvl1))
                {
                    while (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();
                        char[] arrayChars = line.ToArray<char>();
                        board.Add(arrayChars);
                    }
                }  
                break;
            case 2:
                using (StreamReader reader = new StreamReader(pathLvl2))
                {
                    while (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();
                        char[] arrayChars = line.ToArray<char>(); 
                        board.Add(arrayChars);
                    }
                }  
                break;
            case 3:
                using (StreamReader reader = new StreamReader(pathLvl3))
                {
                    while (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();
                        char[] arrayChars = line.ToArray<char>(); 
                        board.Add(arrayChars);
                    }
                }  
                break;
            case 4:
                using (StreamReader reader = new StreamReader(pathLvl4))
                {
                    while (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();
                        char[] arrayChars = line.ToArray<char>(); 
                        board.Add(arrayChars);
                    }
                }  
                break;
            case 5:
                using (StreamReader reader = new StreamReader(pathLvl5))
                {
                    while (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();
                        char[] arrayChars = line.ToArray<char>(); 
                        board.Add(arrayChars);
                    }
                }  
                break;
            default:
            break;
        }

        foreach(char[] iArrayChar in board){
            foreach(char iChar in iArrayChar){
                if(iChar.Equals('X')){
                    totalTiles++;
                }
            }
        }
    }
    
    public void LoadSprites(){
        int setId = Random.Range(0, sets.Count());
        string setName = sets[setId];
        
        foreach(Sprite iSet in Sprites){
            if(iSet.name.Contains(setName)){
                setSprites.Add(iSet);
            }
        }

        int numCuples = totalTiles/2;
        totalTiles = numCuples*2;
        int actualY = 0;
        int actualX = 0;
        int cuplesCounter = 0;
        int actualTilesCreated = 0;
        int singleTilesCreated = 0;
        int fixedSingleTilesCreated = 0;
        
        List<int> listRemaining = new List<int>();
        float xPosition = 0;
        float yPosition = 0;
        Vector2 actualPos = new Vector2();
        
        foreach(char[] iArrayChar in board){
            GameObject[] tempGO = new GameObject[iArrayChar.Count()];
            boardGridTiles[actualX] = new GameObject[20];
            foreach(char iChar in iArrayChar){  
                Vector2 tilePosition = new Vector2(tilePrefab.transform.position.x + xPosition, tilePrefab.transform.position.y + yPosition); 
                GameObject tileCreated = Instantiate(tilePrefab, tilePosition, tilePrefab.transform.rotation);
                tileCreated.transform.parent = gameController.transform;
                tileCreated.GetComponent<TileController>().location = new Vector2(actualY,actualX);
                actualPos.y=actualY;
                actualPos.x=actualX;

                int nextAction = Random.Range(0, 2);
                if(actualTilesCreated == 0 || singleTilesCreated == 0){
                    nextAction = 0;
                }
                
                if(iChar.Equals('X') && nextAction == 0 && fixedSingleTilesCreated < numCuples)
                {
                    int randomSet = Random.Range(0, setSprites.Count());
                    tileCreated.GetComponent<SpriteRenderer>().sprite = setSprites[randomSet];
                    tileCreated.GetComponent<TileController>().tileID = randomSet+1;
                    tileCreated.name = "TileId" + (randomSet+1) +  "-X" + actualX + "-Y" + actualY;

                    listRemaining.Add(randomSet);
                    actualTilesCreated++;
                    fixedSingleTilesCreated++;
                    singleTilesCreated++;
                }
                else if(iChar.Equals('X') && cuplesCounter < numCuples && actualTilesCreated < totalTiles )
                {
                    int positionOfRemaining = Random.Range(0,listRemaining.Count);
                    int posRem = listRemaining[positionOfRemaining];
                    while(posRem<0){
                        positionOfRemaining = Random.Range(0,listRemaining.Count);
                        posRem = listRemaining[positionOfRemaining];
                    }
                    listRemaining[positionOfRemaining] = -1;                    
                    tileCreated.GetComponent<SpriteRenderer>().sprite = setSprites[posRem];
                    tileCreated.GetComponent<TileController>().tileID = posRem+1;
                    tileCreated.name = "TileId" + (posRem+1) +  "-X" + actualX + "-Y" + actualY;

                    
                    //listRemaining.Remove(positionOfRemaining);
                    cuplesCounter++;
                    singleTilesCreated--;
                    actualTilesCreated++; 
                }
                else
                {
                    //give name to the blank tiles
                    tileCreated.name = "TileId" + 0 +  "-X" + actualX + "-Y" + actualY;
                    tileCreated.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,.1f);
                }
                
                boardGridTiles[actualX][actualY] = tileCreated;
                tempGO[actualY] = tileCreated;
                actualY++;
                xPosition+=0.6f;
            }
            maxX = actualY;
            actualX++;
            actualY = 0;
            xPosition = 0;
            yPosition-=0.7f;
            boardTiles.Add(tempGO);

        }
        maxY = actualX;
    }

}
