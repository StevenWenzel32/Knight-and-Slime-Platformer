using UnityEngine;

// saves player data in binary form for security 
public class SaveManager : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static SaveManager instance {get; private set;}
    // for initalizing size
    private int levelCount = 10;
    // where the saved data gets loaded and where new data goes
    public LevelInfo[] levels;

    public void Awake(){
        // singleton stuff
        if (instance == null){
            instance = this;
            // make sure this persists across scenes
            DontDestroyOnLoad(gameObject);
            // initalize the levels to a size
            levels = new LevelInfo[levelCount];
            // since the levelInfo is a class not a struct need to initalize the array before use
            for (int i = 0; i < levelCount; i++)
            {
                levels[i] = new LevelInfo();
                // if the first level make sure it's unlocked
                if (i == 0){
                    levels[i].locked = false;
                }
            }
            // load in the saved PlayerData
            LoadAllLevelData();
        } else {
            Destroy(gameObject);
        }
    }

    // save all of the level data present in levels[] - called when the save button is pressed by the user
    public void SaveAllLevelData(){
        // loop through all the levels and save their data
        for (int i = 0; i < levels.Length; i++){
            // check if a level is locked if it is it still has default data
            if (levels[i].locked){
                break;
            }
            // if the time or gem count is better than the current saved score save the new best
            else if ((levels[i].gems > PlayerPrefs.GetInt($"Level_{i + 1}_Gems")) || (levels[i].time < PlayerPrefs.GetFloat($"Level_{i + 1}_Time"))){
                // save the level data 
                SaveLevelData(levels[i], (i + 1));
            }
        }
    }

    // pass in a LevelInfo and save it's data into the playerPrefs
    // call this when a level is completed
    public void SaveLevelData(LevelInfo levelData, int levelNumber)
    {
        // check if the player got more gems
        if (levelData.gems > PlayerPrefs.GetInt($"Level_{levelNumber}_Gems")){
            // save the data 
            SaveData(levelData, levelNumber);
        } 
        // check if the player had the same amount of gems but a better time
        else if ((levelData.gems == PlayerPrefs.GetInt($"Level_{levelNumber}_Gems")) && (levelData.time < PlayerPrefs.GetFloat($"Level_{levelNumber}_Time"))){
            SaveData(levelData, levelNumber);
        }

        // save them right away
        PlayerPrefs.Save();
    }

    // save the data pieces for the level
    public void SaveData(LevelInfo levelData, int levelNumber){
        // Use keys with the level number as part of the key name
        PlayerPrefs.SetInt($"Level_{levelNumber}_Stars", levelData.stars);
        PlayerPrefs.SetInt($"Level_{levelNumber}_Score", levelData.score);
        PlayerPrefs.SetInt($"Level_{levelNumber}_Gems", levelData.gems);
        PlayerPrefs.SetFloat($"Level_{levelNumber}_Time", levelData.time);
        PlayerPrefs.SetInt($"Level_{levelNumber}_Locked", levelData.locked ? 1 : 0);
    }

    // pass in a LevelInfo and save it's data into the playerPrefs
    // call this when a level is completed
    public void SaveLevelLock(bool locked, int levelNumber)
    {
        // Use keys with the level number as part of the key name
        PlayerPrefs.SetInt($"Level_{levelNumber}_Locked", locked ? 1 : 0);

        // save them right away
        PlayerPrefs.Save();
    }

    // pass in a levelNumber and load that levels data into levels[]
    // called when the game is loaded
    public void LoadAllLevelData()
    {
        // loop through all the levels[] and load their data
        for (int i = 0; i < levels.Length; i++){
            // check if a level is locked if it is it still has default data
            if (PlayerPrefs.GetInt($"Level_{i + 1}_Locked", 1) == 1){
                break;
            }
            levels[i].gems = PlayerPrefs.GetInt($"Level_{i + 1}_Gems", 0);
            levels[i].stars = PlayerPrefs.GetInt($"Level_{i + 1}_Stars", 0);
            levels[i].score = PlayerPrefs.GetInt($"Level_{i + 1}_Score", 0);
            levels[i].time = PlayerPrefs.GetFloat($"Level_{i + 1}_Time", 0f);
            levels[i].locked = PlayerPrefs.GetInt($"Level_{i + 1}_Locked", 1) == 1;
        }
    }
}