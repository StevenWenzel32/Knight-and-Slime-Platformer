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
    public static LevelManager instance { get; private set; }
    // level info var to be set that the one that is being saved 
    // once a level is completed this is where the data is stored before being saved
    public LevelInfo currentLevel;
    // for easy reference to the levelNumber inside the levelCounter
    private int levelNumber;
    // for easy reference to the currrentChapterNumber inside chapterManager
    private int chapterNumber;

    [Header("Display Stats")]
    public TMP_Text levelNum;
    public TMP_Text gemsCollected;
    // currently not used -- for future update ****
    public TMP_Text starsEarned;
    public TMP_Text time;
    public TMP_Text score;

    // The UI canvas the counters are on 
    public GameObject counters;

    [Header("Game Over")]
    // show when a player dies
    public GameObject gameOverScreen;
    // play when game over pops up
    public AudioClip gameOverSound;

    [Header("Level Complete")]
    // show when level is complete
    public GameObject levelCompleteScreen;
    // play when level complete pops up
    public AudioClip levelCompleteSound;
    public GameObject LevelCompleteArrow;
    // tells the player they got a new high score
    public GameObject HighScoreMsg;

    [Header("Pause")]
    public GameObject pauseScreen;
    public GameObject settingsScreen;

    [Header("Options")]
    // for running a cutscene after the level is completed
    public bool hasCutscene;

    // Start is called before the first frame update
    void Start()
    {
        // singleton stuff
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // make sure all the other screens are turned off
        gameOverScreen?.SetActive(false);
        pauseScreen?.SetActive(false);
        levelCompleteScreen?.SetActive(false);
        settingsScreen?.SetActive(false);
        HighScoreMsg.SetActive(false);

        // for easy reference grab the current chapter num
        chapterNumber = LevelSelectManager.instance.currentChapter.chapterNum;
        Debug.Log("chapterNumber = " + chapterNumber);
        // for easy reference to the levelNumber inside the levelInfo
        levelNumber = LevelCounter.instance.levelNumber;
        Debug.Log("levelNumber = " + levelNumber);
        // for easy reference to the current levelInfo inside the levels[]
        currentLevel = LevelSelectManager.instance.chapters[chapterNumber - 1].levels[levelNumber - 1];

        // start timer
        EventManager.OnTimerStart();
    }

    private void Update()
    {
        // toggle the pause screen
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.activeInHierarchy)
            {
                PauseGame(false);
            }
            else
            {
                PauseGame(true);
            }
        }
    }

    // player died
    public void GameOver()
    {
        // turn off in game counters
        counters.SetActive(false);
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    // level completed screen displayed 
    public void LevelComplete()
    {
        // turn off in game counters
        counters.SetActive(false);
        // turn on level complete screen
        levelCompleteScreen.SetActive(true);
        SoundManager.instance.PlaySound(levelCompleteSound);
    }

    // actions to take when the level is completed
    public void LevelCompleted()
    {
        // stop the timer to give acurate score
        EventManager.OnTimerStop();
        // pause the game to save player from dying
        Time.timeScale = 0;
        // do end of level score calcs
        ScoreCounter.instance.EndOfLevelCalc();
        // display the players performance 
        DisplayStats();
        // check if the level has a cutscene to play at the end of the level
        if (hasCutscene)
        {
            // Play video before showing the Level Complete screen
            StartCoroutine(PlayVideoThenCompleteLevel());
        }
        else
        {
            // put up the level complete screen
            LevelComplete();

            // reactivate the selection arrow
            LevelCompleteArrow.SetActive(true);

            // check if the score is better
            if (CheckIfBetterScore())
            {
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
    public void Restart()
    {
        // get the current scene and return it's index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // show the counters again
        counters.SetActive(true);
        // make sure the time is restored after restart
        Time.timeScale = 1;
    }

    // load the nextlevel
    public void NextLevel()
    {
        // get the current scene and return it's index
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel + 1);
        // restore the game time
        Time.timeScale = 1;
    }

    #region Pause
    // toggle the pause menu
    public void PauseGame(bool show)
    {
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
    public void SettingsScreen(bool show)
    {
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
        if (CheckIfBetterScore())
        {
            // display the high score msg
            HighScoreMsg.SetActive(true);
            // saving this level data
            SaveNewLevelData();
        }

        // unlock the next level
        UnlockNextLevel();
    }

    public void DisplayStats()
    {
        levelNum.text = "Level " + levelNumber;
        gemsCollected.text = GemCounter.instance.gemsCollected + "/" + GemCounter.instance.gemsToCollect;
        // convert the float time into a timespan
        TimeSpan timeSpan = TimeSpan.FromSeconds(ScoreCounter.instance.playerTime);
        // set the text object to the timespan and format it
        time.text = timeSpan.ToString(format: @"mm\:ss\:ff");
        score.text = "Score: " + ScoreCounter.instance.score;
        // might add this later -- not needed for now -- better if it was visual -- ****
        // starsEarned.text = "Stars: " + ScoreCounter.instance.stars;
    }

    // update the levelInfo in the levels[] for the level just completed
    private void UpdateLevelInfo()
    {
        currentLevel.gems = GemCounter.instance.gemsCollected;
        currentLevel.time = ScoreCounter.instance.playerTime;
        currentLevel.score = ScoreCounter.instance.score;
        currentLevel.stars = ScoreCounter.instance.stars;
        // if remove lock works correctly this is not needed
        currentLevel.locked = false;
    }

    // check if the player got a new high score
    private bool CheckIfBetterScore()
    {
        bool better = false;
        int gemsOld = LevelSelectManager.instance.chapters[chapterNumber - 1].levels[levelNumber - 1].gems;
        int gemsNew = GemCounter.instance.gemsCollected;
        float newTime = ScoreCounter.instance.playerTime;
        float oldTime = LevelSelectManager.instance.chapters[chapterNumber - 1].levels[levelNumber - 1].time;
        // if more gems collected
        if (gemsNew > gemsOld)
        {
            better = true;
            Debug.Log("In better score check");
            Debug.Log("gems collected = " + gemsNew + ", gems previous collected = " + gemsOld);
        }
        // if same gems but better time
        else if ((gemsNew == gemsOld) && (newTime < oldTime))
        {
            better = true;
            Debug.Log("new time = " + newTime + ", old time = " + oldTime);
        }

        return better;
    }

    // save the new high score 
    private void SaveNewLevelData()
    {
        Debug.Log("Player got a new high score!");
        // display a new high score message to the player --- ****?????

        // update the level data in the levels[] aka currentLevel
        UpdateLevelInfo();
        // have the saveManager save the new current level data to playerPrefs
        SaveManager.instance.SaveLevelData(currentLevel, levelNumber);

        //update the chapter info in the chapters[] aka currentChapter **********************************

        // have the saveManager save the new currentChapter data to playerPrefs ***********************************

    }

    // unlock the next level if it isn't already unlocked
    private void UnlockNextLevel()
    {
        // check if the lock is active and if levelNumber is the last level, and only change save data if it is
        if (levelNumber != LevelSelectManager.instance.chapters[chapterNumber - 1].levels.Length && LevelSelectManager.instance.chapters[chapterNumber - 1].levels[levelNumber + 1].locked)
        {
            Debug.Log("Changing lock data for level: " + (levelNumber + 1));
            // turn off the lock for the next level in the LevelInfo
            LevelSelectManager.instance.chapters[chapterNumber - 1].levels[levelNumber + 1].locked = false;
            // save just the lock change for the next level to playerPrefs
            SaveManager.instance.SaveLevelLock(false, levelNumber + 1);
        }
    }

    //getters and setters
    public int getLevelNumber()
    {
        return levelNumber;
    }
}
