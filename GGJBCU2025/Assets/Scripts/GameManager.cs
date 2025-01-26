using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool winner = false;
    public TextMeshProUGUI WinText;
    public bool Player1Win = false;
    public bool Player2Win = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void Play() //  causes main game to play
    {
        SceneManager.LoadScene(1);
    }


    public void Quit() // exits game 
    {
        Application.Quit(0);
    }
     public void Win() // transisions to winning scene
    {
        SceneManager.LoadScene(2);
        winner = false;
        
        if (Player1Win == true)
        {
            Debug.Log("Congratulations Player 1 Duck");
            WinText.text = "Congratulations Player 1 Duck";
        }
        else if (Player2Win == true)
        {
            Debug.Log("Congratulations Player 2 Duck");
            WinText.text = "Congratulations Player 2 Duck";
        }
    

}

    public void ResetGame() { // reset to the games title screen
        SceneManager.LoadScene(0);
        Debug.Log("reset presses i guess");
    }

    // Update is called once per frame
    void Update()
    {
        if (winner) { Win(); }
    }
}
