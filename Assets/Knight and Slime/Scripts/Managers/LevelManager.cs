using TMPro;
using UnityEngine;
using System;
using System.Collections;

// each level is to have a levelManager object on it 
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


    // Start is called before the first frame update
    void Start()
    {
        // singleton stuff
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }

        levelNumber = LevelCounter.instance.levelNumber;
        // start timer
        EventManager.OnTimerStart();
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
        // Play video before showing the Level Complete screen
        StartCoroutine(PlayVideoThenCompleteLevel());
    }

    private IEnumerator PlayVideoThenCompleteLevel()
    {
        // Enable and play the video
        VideoManager.instance.PlayVideo(); // Assuming you have a VideoManager script
        
        // Wait until the video finishes
        while (VideoManager.instance.IsPlaying())
        {
            // Wait for the next frame
            yield return null; 
        }

        // put up the level complete screen
        MenuManager.instance.LevelComplete();

        // saving this level data
        // check if the score is better
        if (checkIfBetterScore()){
            Debug.Log("Player got a new high score!");
            // send the level data from the counters to the levelInfo which is currently empty
            updateLevelInfo();
            // save the levelInfo to the array in the saveManager
            SaveManager.instance.levels[levelNumber - 1] = levelInfo;
            // have the saveManager save the new current level data to playerPrefs
            SaveManager.instance.SaveLevelData(levelInfo, levelNumber);
        }

        // unlocking the next level
        // check if the lock is active and if levelNumber is the last level, and only change save data if it is
        if (levelNumber != SaveManager.instance.levels.Length && SaveManager.instance.levels[levelNumber + 1].locked){
            Debug.Log("Changing lock data for level: " + (levelNumber + 1));
            // turn off the lock for the next level in the LevelInfo
            SaveManager.instance.levels[levelNumber].locked = false;
            // save just the lock change for the next level to playerPrefs
            SaveManager.instance.SaveLevelLock(false, levelNumber + 1);
        }
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
    private void updateLevelInfo(){
        levelInfo.gems = GemCounter.instance.gemsCollected;
        levelInfo.time = ScoreCounter.instance.playerTime;    
        levelInfo.score = ScoreCounter.instance.score;
        levelInfo.stars = ScoreCounter.instance.stars;
        // if remove lock works correctly this is not needed
        levelInfo.locked = false;
    }

    private bool checkIfBetterScore(){
        bool better = false;
        int gemsOld = SaveManager.instance.levels[levelNumber - 1].gems;
        int gemsNew = GemCounter.instance.gemsCollected;
        // if more gems collected
        if (gemsNew > gemsOld){
            better = true;
            Debug.Log("In better score check");
            Debug.Log("gems collected = " + gemsNew + ", gems previous collected = " + gemsOld);
        } 
        // if same gems but better time
        else if ((gemsNew == gemsOld) && (ScoreCounter.instance.playerTime < SaveManager.instance.levels[levelNumber - 1].time)){
            better = true;
        }

        return better;
    }
}
