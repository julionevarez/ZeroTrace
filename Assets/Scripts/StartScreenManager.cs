using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    // Called by Start Mission button — loads the main game scene
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}