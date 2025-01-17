using UnityEngine;
using UnityEngine.SceneManagement;

// should only be used for the main menu and the other menus that can only be accessed there - name change will happen later
public class MainMenuManager : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static MainMenuManager instance {get; private set;}

    [Header ("Scene Management")]
    // the scene # that the game should start at
    public int startScene;

    [Header ("Screens")]
    public GameObject guideScreen;
    public GameObject creditsScreen;

    [Header ("Other")]
    public GameObject selectionArrow;

    private void Awake(){
        // singleton stuff
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }

        // make sure any other screens are turned off based on the menu type
        guideScreen?.SetActive(false);
    }

    // sends the player to the beginning scene of the game -- in this case the main menu
    public void StartScene()
    {
        // loads the given scene
        SceneManager.LoadScene(startScene);
    }

    // quit the game - button func
    public void QuitGame()
    {
        // works in build
        Application.Quit();   
        #if UNITY_EDITOR
        // works in play/editor
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // go to the settings menu/scene
    public void SettingsMenu(){
        SceneManager.LoadScene(1);
    }

    // go to the level select menu/scene
    public void LevelSelectMenu(){
        SceneManager.LoadScene(2);
    }

    // show the guide screen overlay
    public void GuideScreen(bool show){
        if (guideScreen != null)
        {
            // turn on the guide screen
            guideScreen.SetActive(show);
        }
    }

    // show the credits guide screen overlay
    public void CreditsScreen(bool show){
        if (creditsScreen != null)
        {
            // turn on the guide screen
            creditsScreen.SetActive(show);
        }
    }
}
