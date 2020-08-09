using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleController : MonoBehaviour
{
    public static int lvlLoaded = 1;
    public GameObject[] levelsCompleted;
    public TextMeshProUGUI[] topScoresText;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        LoadData();        
    }


    public void LoadLevel(int level){
        lvlLoaded = level;
        SceneManager.LoadScene("GameScene",LoadSceneMode.Single);
    }

    public void LoadData(){
        PlayerData playerData = SaveSystem.LoadPlayer();
        Debug.Log("Loading... ");   
        if(playerData!=null){
            int[] topScores = playerData.topScores;
            for(int i = 0; i<topScores.Length; i++){
                if(topScores[i]>0){
                    levelsCompleted[i].GetComponent<Image>().color = new Color(1f,1f,1f,1f);
                    topScoresText[i].SetText(""+topScores[i]);
                }else{
                    topScoresText[i].SetText("0");
                }
            }
        }

        if(playerData.soundIsOn){
            audioSource.volume = 0.10f;
        }else{
            audioSource.volume = 0f;
        }
    }

    public void CloseAll() {
        #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            Debug.Log(this.name+" : "+this.GetType()+" : "+System.Reflection.MethodBase.GetCurrentMethod().Name); 
        #endif
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE) 
            Application.Quit();
        #elif (UNITY_WEBGL)
            Application.OpenURL("about:blank");
        #endif
    }
}
