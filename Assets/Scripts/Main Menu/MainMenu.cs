using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private bool isGameStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if(!isGameStarted && Input.anyKeyDown 
        && !Input.GetMouseButtonDown(0) 
        && !Input.GetMouseButtonDown(1) 
        && !Input.GetMouseButtonDown(2)){
            isGameStarted = true;
            SceneManager.LoadScene("SampleScene");
        }
    } 

    public void QuitGame(){
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
