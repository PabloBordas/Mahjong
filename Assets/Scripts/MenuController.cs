using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool SoundIsOn = true;
    public GameObject Menu;
    public GameObject WinPopup;
    public GameObject LosePopup;
    public GameObject newTopScore;
    public GameObject menuInGame;
    public TextMeshProUGUI soundText;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI topScoreText;
    public AudioSource audioSource;
    public AudioClip menuSound;
    public AudioClip exitSound;
    public AudioClip winSound;
    public float volumen = 1f;
    public GameController gameController;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume(){
        audioSource.PlayOneShot(menuSound, volumen);
        Menu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause(){
        CheckSound();
        audioSource.PlayOneShot(menuSound, volumen);
        Menu.SetActive(true);        
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Exit(){
        menuInGame.SetActive(true);        
        audioSource.PlayOneShot(exitSound, volumen);
        GameIsPaused = false;
        SceneManager.LoadScene("TitleScene",LoadSceneMode.Single);
    }

    public void Win(int score, int lastTopScore){
        menuInGame.SetActive(false);
        CheckSound();
        audioSource.PlayOneShot(winSound, volumen);
        WinPopup.SetActive(true);
        if(score>lastTopScore){
            newTopScore.SetActive(true);
            topScoreText.SetText("Top Score: " + score);
        }else{
            newTopScore.SetActive(false);
            topScoreText.SetText("Top Score: " + lastTopScore);
        }   
        playerScoreText.SetText("Your Score: " + score);
        
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Lose(){
        menuInGame.SetActive(false);
        audioSource.PlayOneShot(exitSound, 1f);
        LosePopup.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void CheckSound(){
        if(!gameController.soundIsOn){
            SoundIsOn = false;
            volumen = 0f;
            gameController.audioSource.volume = 0f;
            soundText.SetText("Sound: Off");
        }else{
            SoundIsOn = true;
            volumen = 1f;
            gameController.audioSource.volume = 0.10f;
            soundText.SetText("Sound: On");
        }
    }

    public void Sound(){
        
        if(!gameController.soundIsOn){
            gameController.soundIsOn = true;
            SoundIsOn = true;
            volumen = 1f;
            gameController.audioSource.volume = 0.10f;
            soundText.SetText("Sound: ON");
        }else{
            gameController.soundIsOn = false;
            SoundIsOn = false;
            volumen = 0f;
            gameController.audioSource.volume = 0f;
            soundText.SetText("Sound: OFF");
        }
        gameController.SaveData(false);     
    }

}
