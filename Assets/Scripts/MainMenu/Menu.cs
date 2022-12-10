using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void NewGame() {
        PlayerPrefs.SetInt("Checkpoint", 0);
        PlayerPrefs.SetInt("Level", 1);
        Continue();
    }
    public void Continue() {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
    }
    public void Debug() {
        SceneManager.LoadScene(2);
    }
}
