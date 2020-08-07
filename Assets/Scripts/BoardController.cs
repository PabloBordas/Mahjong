using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class BoardController : MonoBehaviour
{
    public int lvlLoaded = 1;
    public string pathLvl1 = "Assets/TestLayouts/11x11 [abstract].txt";
    public string pathLvl2 = "Assets/TestLayouts/11X12 - [up].txt";
    public string pathLvl3 = "Assets/TestLayouts/12X12 - [#].txt";
    public string pathLvl4 = "Assets/TestLayouts/13X11 - [1].txt";
    public string pathLvl5 = "Assets/TestLayouts/13x14 - [caro].txt";
    public List<char[]> board = new List<char[]>();
    public List<GameObject[]> boardTiles = new List<GameObject[]>();
    public GameObject tilePrefab;
    public int totalTiles = 0;
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
                        Debug.Log(line);
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
                        Debug.Log(line);
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
                        Debug.Log(line);
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
                        Debug.Log(line);
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
                        Debug.Log(line);
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

        Debug.Log(totalTiles);
    }
    
    public void LoadSprites(){
        int setId = Random.Range(0, sets.Count());
        string setName = sets[setId];
        
        foreach(Sprite iSet in Sprites){
            if(iSet.name.Contains(setName)){
                setSprites.Add(iSet);
                Debug.Log(iSet.name);
            }
        }

        int numCuples = totalTiles/2;
        int numCuplesRemaining = numCuples;
        int column = 0;
        int row = 0;
        int cuplesCounter = 0;
        Debug.Log("numCuples: " + numCuples);
        List<int> listRemaining = new List<int>();
        float xPosition = 0;
        float yPosition = 0;
        foreach(char[] iArrayChar in board){
            GameObject[] tempGO = new GameObject[iArrayChar.Count()];
            foreach(char iChar in iArrayChar){  
                Vector2 tilePosition = new Vector2(tilePrefab.transform.position.x + xPosition, tilePrefab.transform.position.y + yPosition); 
                GameObject tileCreated = Instantiate(tilePrefab, tilePosition, tilePrefab.transform.rotation);
                tileCreated.transform.parent = gameController.transform;
                tileCreated.GetComponent<TileController>().row = row;
                tileCreated.GetComponent<TileController>().column = column;
                int nextAction = Random.Range(0, 2);
                if(cuplesCounter == 0){
                    nextAction = 0;
                }else{
                    nextAction = Random.Range(0, 2);
                }
                
                if(iChar.Equals('X') && nextAction == 0 && numCuplesRemaining >= 0){
                    int randomSet = Random.Range(0, setSprites.Count());
                    tileCreated.GetComponent<SpriteRenderer>().sprite = setSprites[randomSet];
                    tileCreated.GetComponent<TileController>().tileID = randomSet+1;
                    numCuplesRemaining--;
                    listRemaining.Add(randomSet);
                }else if(iChar.Equals('X') && cuplesCounter < numCuples){
                    int positionOfRemaining = Random.Range(0,listRemaining.Count);
                    int posRem = listRemaining[positionOfRemaining];
                    tileCreated.GetComponent<SpriteRenderer>().sprite = setSprites[posRem];
                    tileCreated.GetComponent<TileController>().tileID = posRem+1;
                    listRemaining.Remove(positionOfRemaining);
                    cuplesCounter++;
                }
                tempGO[column] = tileCreated;
                column++;
                xPosition+=0.6f;
            }
            row++;
            column = 0;
            xPosition = 0;
            yPosition-=0.7f;
            boardTiles.Add(tempGO);

        }
    }
}
