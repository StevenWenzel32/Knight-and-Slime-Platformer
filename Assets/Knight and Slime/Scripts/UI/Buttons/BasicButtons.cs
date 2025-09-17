using UnityEngine;
using UnityEngine.SceneManagement; // For loading scenes

// class to hold basic button functions, to be used by menuManagers and other managers in general
// this is a static class
public class BasicButtons : MonoBehaviour
{
    // go to the main menu - it's a scene
    public static void GoToMainMenu()
    {
        // make sure the time is restored -- bug when leaving via level complete or pause
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    // the level buttons will call this function when clicked
    public static void StartLevel(int levelNumber)
    {
        LevelSelectManager.instance.SelectLevel(levelNumber);
    }
}
