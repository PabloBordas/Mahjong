using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public static int lvlLoaded = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(int level){
        lvlLoaded = level;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
