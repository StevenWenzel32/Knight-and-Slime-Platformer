using TMPro;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

// each level is to have a levelManager object on it 
// handles displaying the stats for each level
// handles the in level pause menu, and all game end menus
public class LevelManager : MonoBehaviour
{
    // only have one instance ever in the scene, can get else where but can only set here -- singleton -- if you use don't destroy on load it will last across scenes
    public static LevelManager instance {get; private set;}
    // level info var to be set that the one that is being saved 
    public LevelInfo levelInfo = new LevelInfo();
    private int levelNumber;

    [Header ("Display Stats")]
    public TMP_Text levelNum;
    public TMP_Text gemsCollected;
    // currently not used -- for future update ****
    public TMP_Text starsEarned;
    public TMP_Text time;
    public TMP_Text score;
    
    // The UI canvas the counters are on 
    public GameObject counters;

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
    public GameObject LevelCompleteArrow;
    // tells the player they got a new high score
    public GameObject HighScoreMsg;

    [Header ("Pause")]
    public GameObject pauseScreen;
    public GameObject settingsScreen;

    [Header ("Options")]
    // for running a cutscene after the level is completed
    public bool hasCutscene;

    // Start is called before the first frame update
    void Start()
    {
        // singleton stuff
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }
        // make sure all the other screens are turned off
        gameOverScreen?.SetActive(false);
        pauseScreen?.SetActive(false);
        levelCompleteScreen?.SetActive(false);
        settingsScreen?.SetActive(false);
        HighScoreMsg.SetActive(false);

        levelNumber = LevelCounter.instance.levelNumber;
        // start timer
        EventManager.OnTimerStart();
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

    // actions to take when the level is completed
    public void LevelCompleted(){
        // stop the timer to give acurate score
        EventManager.OnTimerStop();
        // pause the game to save player from dying
        Time.timeScale = 0;
        // do end of level score calcs
        ScoreCounter.instance.EndOfLevelCalc();
        // display the players performance 
        DisplayStats();
        // check if the level has a cutscene to play at the end of the level
        if (hasCutscene){
            // Play video before showing the Level Complete screen
            StartCoroutine(PlayVideoThenCompleteLevel());
        } else {
            // put up the level complete screen
            LevelComplete();

            // reactivate the selection arrow
            LevelCompleteArrow.SetActive(true);

            // check if the score is better
            if (CheckIfBetterScore()){
                // display the high score msg
                HighScoreMsg.SetActive(true);
                // saving this level data
                SaveNewLevelData();
            }

            // unlock the next level
            UnlockNextLevel();
        }
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

    // load the nextlevel
    public void NextLevel(){
        // get the current scene and return it's index
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel + 1);
        // restore the game time
        Time.timeScale = 1;
    }

    #region Pause
    // toggle the pause menu
    public void PauseGame(bool show){
        // check if the pause screen exsists, check if the game over or level complete screen is active
        if (pauseScreen != null && !gameOverScreen.activeSelf && !levelCompleteScreen.activeSelf)
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

    private IEnumerator PlayVideoThenCompleteLevel()
    {
        // disable the selection arrow on the levelcomplete screen
        LevelCompleteArrow.SetActive(false);

        // Enable and play the video
        VideoManager.instance.PlayVideo(); // Assuming you have a VideoManager script
        
        // Wait until the video finishes
        while (!VideoManager.instance.isVideoFinished)
        {
            // Wait for the next frame
            yield return null; 
        }
        Debug.Log("video has finished");
        
        // put up the level complete screen
        LevelComplete();

        // reactivate the selection arrow
        LevelCompleteArrow.SetActive(true);

        // check if the score is better
        if (CheckIfBetterScore()){
            // display the high score msg
            HighScoreMsg.SetActive(true);
            // saving this level data
            SaveNewLevelData();
        }

        // unlock the next level
        UnlockNextLevel();
    }

    public void DisplayStats(){
        levelNum.text = "Level " + levelNumber;
        gemsCollected.text = GemCounter.instance.gemsCollected + "/" + GemCounter.instance.gemsToCollect;
        // convert the float time into a timespan
        TimeSpan timeSpan = TimeSpan.FromSeconds(ScoreCounter.instance.playerTime);
        // set the text object to the timespan and format it
        time.text = timeSpan.ToString(format:@"mm\:ss\:ff");
        score.text = "Score: " + ScoreCounter.instance.score;
        // might add this later -- not needed for now -- better if it was visual -- ****
        // starsEarned.text = "Stars: " + ScoreCounter.instance.stars;
    }

    // update the levelInfo for the level just completed
    private void UpdateLevelInfo(){
        levelInfo.gems = GemCounter.instance.gemsCollected;
        levelInfo.time = ScoreCounter.instance.playerTime;    
        levelInfo.score = ScoreCounter.instance.score;
        levelInfo.stars = ScoreCounter.instance.stars;
        // if remove lock works correctly this is not needed
        levelInfo.locked = false;
    }

    // check if the player got a new high score
    private bool CheckIfBetterScore(){
        bool better = false;
        int gemsOld = SaveManager.instance.levels[levelNumber - 1].gems;
        int gemsNew = GemCounter.instance.gemsCollected;
        float newTime = ScoreCounter.instance.playerTime;
        float oldTime = SaveManager.instance.levels[levelNumber - 1].time;
        // if more gems collected
        if (gemsNew > gemsOld){
            better = true;
            Debug.Log("In better score check");
            Debug.Log("gems collected = " + gemsNew + ", gems previous collected = " + gemsOld);
        } 
        // if same gems but better time
        else if ((gemsNew == gemsOld) && (newTime < oldTime)){
            better = true;
            Debug.Log("new time = " + newTime + ", old time = " + oldTime);
        }

        return better;
    }

    // save the new high score 
    private void SaveNewLevelData(){
        Debug.Log("Player got a new high score!");
        // display a new high score message to the player

        // send the level data from the counters to the levelInfo which is currently empty
        UpdateLevelInfo();
        // save the levelInfo to the array in the saveManager
        SaveManager.instance.levels[levelNumber - 1] = levelInfo;
        // have the saveManager save the new current level data to playerPrefs
        SaveManager.instance.SaveLevelData(levelInfo, levelNumber);
    }

    // unlock the next level if it isn't already unlocked
    private void UnlockNextLevel(){
        // check if the lock is active and if levelNumber is the last level, and only change save data if it is
        if (levelNumber != SaveManager.instance.levels.Length && SaveManager.instance.levels[levelNumber + 1].locked){
            Debug.Log("Changing lock data for level: " + (levelNumber + 1));
            // turn off the lock for the next level in the LevelInfo
            SaveManager.instance.levels[levelNumber].locked = false;
            // save just the lock change for the next level to playerPrefs
            SaveManager.instance.SaveLevelLock(false, levelNumber + 1);
        }
    }
}
