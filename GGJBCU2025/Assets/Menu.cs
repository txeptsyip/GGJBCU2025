using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void Start_Game() {

        SceneManager.LoadSceneAsync("MainScene");
    }
}
