using UnityEngine;

// saves player data in binary form for security 
// read and writes the game data for levels and chapters
public class SaveManager : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static SaveManager instance {get; private set;}
    // chapters will always have 10 levels
    const int NUM_OF_LEVELS = 10;

    public void Awake(){
        // singleton stuff
        if (instance == null){
            instance = this;
            // make sure the save manager persists across scenes
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

#region Level Data
    // save all of the level data present in levels[] - called when the save button is pressed by the user
    public void SaveAllLevelData(){
        // loop through all the levels and save their data
        for (int i = 0; i < NUM_OF_LEVELS; i++){
            // check if a level is locked if it is it still has default data
            if (LevelSelectManager.instance.chapter.levels[i].locked){
                break;
            }
            // if the time or gem count is better than the current saved score save the new best
            else if ((LevelSelectManager.instance.chapter.levels[i].gems > PlayerPrefs.GetInt($"Level_{i + 1}_Gems")) || (LevelSelectManager.instance.chapter.levels[i].time < PlayerPrefs.GetFloat($"Level_{i + 1}_Time"))){
                // save the level data 
                SaveLevelData(LevelSelectManager.instance.chapter.levels[i], (i + 1));
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
            // save the updated chapter info
//            SaveChapterData(ChapterInfo chapterData, int chapterNumber);
        } 
        // check if the player had the same amount of gems but a better time
        else if ((levelData.gems == PlayerPrefs.GetInt($"Level_{levelNumber}_Gems")) && (levelData.time < PlayerPrefs.GetFloat($"Level_{levelNumber}_Time"))){
            SaveData(levelData, levelNumber);
 //           SaveChapterData(ChapterInfo chapterData, int chapterNumber)
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

    // call this when a level is completed
    public void SaveLevelLock(bool locked, int levelNumber)
    {
        // Use keys with the level number as part of the key name
        PlayerPrefs.SetInt($"Level_{levelNumber}_Locked", locked ? 1 : 0);

        // save them right away
        PlayerPrefs.Save();
    }

    // pass in a levelNumber and load that levels data into levels[]
    // called when the game is loaded -- can switch to being loaded when the chapter is accessed
    public void LoadAllLevelData()
    {
        // loop through all the levels[] and load their data
        for (int i = 0; i < NUM_OF_LEVELS; i++){
            // check if a level is locked if it is it still has default data
            if (PlayerPrefs.GetInt($"Level_{i + 1}_Locked", 1) == 1){
                break;
            }
            LevelSelectManager.instance.chapter.levels[i].gems = PlayerPrefs.GetInt($"Level_{i + 1}_Gems", 0);
            LevelSelectManager.instance.chapter.levels[i].stars = PlayerPrefs.GetInt($"Level_{i + 1}_Stars", 0);
            LevelSelectManager.instance.chapter.levels[i].score = PlayerPrefs.GetInt($"Level_{i + 1}_Score", 0);
            LevelSelectManager.instance.chapter.levels[i].time = PlayerPrefs.GetFloat($"Level_{i + 1}_Time", 0f);
            LevelSelectManager.instance.chapter.levels[i].locked = PlayerPrefs.GetInt($"Level_{i + 1}_Locked", 1) == 1;
        }
    }

#endregion

#region Chapter Data
    // pass in a ChapterInfo and save it's data into the playerPrefs
    // should be called when ever save data is called
    public void SaveChapterData(ChapterInfo chapterData, int chapterNumber){
        // Use keys with the level number as part of the key name
        // save the data for the chapter
        PlayerPrefs.SetInt($"Chapter_{chapterNumber}_Levels", chapterData.levelsCompleted);
        PlayerPrefs.SetInt($"Chapter_{chapterNumber}_Gems", chapterData.gemsCollected);
        PlayerPrefs.SetInt($"Chapter_{chapterNumber}_Percent", chapterData.percentCompleted);
        PlayerPrefs.SetInt($"Chapter_{chapterNumber}_Locked", chapterData.locked ? 1 : 0);
        // save them right away
        PlayerPrefs.Save();
    }

    // load chapter data until chapters[] is full
    // called when the game is loaded
    public void LoadAllChapterData(ChapterInfo chapter){
        // loop through all the chapters[] and load their data
        for (int i = 0; i < ChapterManager.instance.chapters.Length; i++){
            // check if a chapter is locked, if so it still has default data
            if (PlayerPrefs.GetInt($"Chapter_{i + 1}_Locked", 1) == 1){
                break;
            }
            ChapterManager.instance.chapters[i].levelsCompleted = PlayerPrefs.GetInt($"Chapter_{i + 1}_Levels", 0);
            ChapterManager.instance.chapters[i].gemsCollected = PlayerPrefs.GetInt($"Chapter_{i + 1}_Gems", 0);
            ChapterManager.instance.chapters[i].percentCompleted = PlayerPrefs.GetInt($"Chapter_{i + 1}_Percent", 0);
            ChapterManager.instance.chapters[i].locked = PlayerPrefs.GetInt($"Chapter_{i + 1}_Locked", 1) == 1;

            // setup the chapters levels array
            chapter.InitalizeLevelsArray();

            // this location is only a temporary solution *** once the chapters array is no longer the size of 1 this will no longer work... maybe?? ***
            // load in some of the saved PlayerData
            // load the level data for this chapter till the chapters levels[] is full
            LoadAllLevelData();
        }
    }
    
#endregion     
}