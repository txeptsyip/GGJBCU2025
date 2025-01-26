using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonscript : MonoBehaviour
{

    private GameObject musicobject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicobject = GameObject.FindGameObjectWithTag("Music");
    }

    public void Play() //  causes main game to play
    {
        Destroy(musicobject);
        SceneManager.LoadScene(1);

    }

    public void ResetGame()
    { // reset to the games title screen
        Destroy(musicobject);
        SceneManager.LoadScene(0);
        Debug.Log("reset presses i guess");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
