using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static MenuManager instance {get; private set;}

    [Header ("Scene Management")]
    // the number of the game menu scene
    public int gameScene;

    [Header ("Game Over")]
    // show when a player dies
    public GameObject gameOverScreen;
    // play when game over pops up
    public AudioClip gameOverSound;

    [Header ("Level Complete")]
    // show when level is complete
    public GameObject levelCompleteScreen;
    // play when level complete pops up
    public AudioClip levelCompleteSound;

    [Header ("Pause")]
    public GameObject pauseScreen;
    public GameObject settingsScreen;

    [Header ("Other")]
    // ref counters
    public GameObject counters;
    // ref playerBase
    public PlayerBase playerBase;

    private void Awake(){
        // singleton stuff
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }

        gameOverScreen?.SetActive(false);
        pauseScreen?.SetActive(false);
        levelCompleteScreen?.SetActive(false);
        settingsScreen?.SetActive(false);
    }

    private void Update(){
        // toggle the pause screen
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (pauseScreen.activeInHierarchy){
                PauseGame(false);
            } else {
                PauseGame(true);
            }
        }
    }
    // sends the player to the designated scene
    public void StartScene()
    {
        // loads the given scene
        SceneManager.LoadScene(gameScene);
    }

    // sends the player to the designated scene
    public void LoadLevel(int levelNumber)
    {
        // loads the given scene
        SceneManager.LoadScene(levelNumber + 2);
    }

    // quit the game - to be used in the quit button
    public void QuitGame()
    {
        // works in build
        Application.Quit();   
        #if UNITY_EDITOR
        // works in play/editor
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // player died
    public void GameOver(){
        // turn off in game counters
        counters.SetActive(false);
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    // level completed screen displayed 
    public void LevelComplete(){
        // turn off in game counters
        counters.SetActive(false);
        // turn on level complete screen
        levelCompleteScreen.SetActive(true);
        SoundManager.instance.PlaySound(levelCompleteSound);
    }

    // for retry level button
    public void Restart(){
        // get the current scene and return it's index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // show the counters again
        counters.SetActive(true);
        // make sure the time is restored after restart
        Time.timeScale = 1;
    }

    // go to the main menu
    public void MainMenu(){
        // make sure the time is restored -- bug when leaving via level complete or pause
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    // go to the first level -- start the game -- replace with a level select menu later **
    public void StartGame(){
       SceneManager.LoadScene(3);
    }

    // go to the settings
    public void SettingsMenu(){
        SceneManager.LoadScene(1);
    }

    public void LevelSelectMenu(){
        SceneManager.LoadScene(2);
    }

    #region Pause
    // toggle the pause menu
    public void PauseGame(bool show){
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(show);
            // turn off the counters
            counters.SetActive(!show);
            // Pause/unpause the game by stoping time
            Time.timeScale = show ? 0 : 1;
        }
    }

    // go to the settings
    public void SettingsScreen(bool show){
        if (settingsScreen != null)
        {
            // turn on the settings screen
            settingsScreen.SetActive(show);
            // turn off the pause screen
            pauseScreen.SetActive(false);
            // Pause/unpause the game by stoping time
            Time.timeScale = show ? 0 : 1;
        }
    }
    #endregion
}