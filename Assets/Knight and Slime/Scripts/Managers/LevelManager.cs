using TMPro;
using UnityEngine;
using System;

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
        // put up the level complete screen
        MenuManager.instance.LevelComplete();

        // send the level data from the counters to the levelInfo 
        updateLevelInfo();
        // update the stars on the level Select Menu -- also calls the removeLock for the next level
        LevelSelectManager.instance.UpdateStars(levelInfo.stars);
        // save the levelInfo to the array in the saveManager
        SaveManager.instance.levels[levelNumber - 1] = levelInfo;
        // have the saveManager save the level data to playerPrefs
        SaveManager.instance.SaveLevelData(levelInfo, levelNumber);
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
}
