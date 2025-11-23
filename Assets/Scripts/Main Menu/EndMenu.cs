using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{

    private bool isGameStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameStarted && Input.anyKeyDown 
        && !Input.GetMouseButtonDown(0) 
        && !Input.GetMouseButtonDown(1) 
        && !Input.GetMouseButtonDown(2)){
            RestartGame();
            isGameStarted = true;
            SceneManager.LoadScene("SampleScene");
        }
    } 

    public void RestartGame(){
        // reset the game and its variables here
        Debug.Log("Restarting Game");
    }

    public void QuitGame(){
        Application.Quit();
        Debug.Log("Quitting Game");
    }

}

