using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonscript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Play() //  causes main game to play
    {
        SceneManager.LoadScene(1);
    }

    public void ResetGame()
    { // reset to the games title screen
        SceneManager.LoadScene(0);
        Debug.Log("reset presses i guess");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
