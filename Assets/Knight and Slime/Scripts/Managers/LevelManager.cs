using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // only have one instance ever in the scene, can get else where but can only set here -- singleton -- if you use don't destroy on load it will last across scenes
    public static LevelManager instance {get; private set;}
    // level info var to be set that the one that is being saved 
    public LevelInfo levelInfo = new LevelInfo();
    public int levelNumber;

    // Start is called before the first frame update
    void Start()
    {
        // singleton stuff
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }

        // start timer
        EventManager.OnTimerStart();
    }

    // level completed screen displayed 
    public void LevelCompleted(){
        // stop the timer to give acurate score
        EventManager.OnTimerStop();
        // pause the game to save player from dying
        Time.timeScale = 0;
        // put up the level complete screen
        MenuManager.instance.LevelComplete();
        // do end of level score calcs
        ScoreCounter.instance.EndOfLevelCalc();

        // saving related stuff
        // send the level completion info to the levelInfo 
        updateLevelInfo();
        // save the updated levelInfo to the array in the saveManager
        SaveManager.instance.levels[levelNumber - 1] = levelInfo;
        SaveManager.instance.SaveAll();
    }

    // update the levelInfo for the level just completed
    private void updateLevelInfo(){
        // set the level #
        levelInfo.levelNumber = LevelCounter.instance.levelNumber;
        levelInfo.gems = GemCounter.instance.gemsCollected;
        levelInfo.time = ScoreCounter.instance.playerTime;    
        levelInfo.score = ScoreCounter.instance.score;
        levelInfo.stars = ScoreCounter.instance.stars;
        levelInfo.locked = false;
        
    }
}
