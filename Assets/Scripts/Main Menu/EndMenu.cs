using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndMenu : MonoBehaviour
{

    private bool isGameStarted = false;
    private bool canDetectInput = false; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private IEnumerator Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // wait 10 seconds before enabling input
        yield return new WaitForSeconds(10f);
        canDetectInput = true;
    }

    // Update is called once per frame
    void Update()
    {   
        if (!canDetectInput) return; 
        
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

