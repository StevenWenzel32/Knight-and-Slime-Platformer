using System.IO;
using UnityEngine;

// saves player data in binary form for security 
// read and writes the game data for levels and chapters
public class SaveManager : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static SaveManager instance { get; private set; }
    // chapters will always have 10 levels
    const int NUM_OF_LEVELS = 10;
    
    public void Awake()
    {
        // singleton stuff
        if (instance == null)
        {
            instance = this;
            // make sure the save manager persists across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Everything
    // is called when the save button is pressed by the user
    public void SaveEverything()
    {
        // loop through all the chapters and save their level data
        for (int i = 0; i < LevelSelectManager.instance.chapters.Length; i++)
        {
            // save all the level data
            SaveAllLevelData(LevelSelectManager.instance.chapters[i]);

            // save all the chapter data
            // check if a chapter is locked, if it is it still has default data
            if (LevelSelectManager.instance.chapters[i].locked)
            {
                break;
            }
            // if the percent completed is better then save over the old data -- might not be working if the percent isn't working right*********
            else if (LevelSelectManager.instance.chapters[i].percentCompleted > PlayerPrefs.GetInt($"Chapter_{i + 1}_Percent"))
            {
                // save the chapter data
                SaveChapterData(LevelSelectManager.instance.chapters[i], (i + 1));
            }
        }
    }

    #endregion Everything

    #region Level Data
    // save all of the level data present in the levels[] for the chapter passed in
    public void SaveAllLevelData(ChapterInfo chapter)
    {
        // loop through all the levels and save their data
        for (int i = 0; i < NUM_OF_LEVELS; i++)
        {
            // check if a level is locked if it is it still has default data
            if (chapter.levels[i].locked)
            {
                break;
            }
            // if the time or gem count is better than the current saved score save the new best
            else if ((chapter.levels[i].gems > PlayerPrefs.GetInt($"Chapter_{chapter.chapterNum}_Level_{i + 1}_Gems")) || (chapter.levels[i].time < PlayerPrefs.GetFloat($"Chapter_{chapter.chapterNum}_Level_{i + 1}_Time")))
            {
                // save the level data 
                SaveLevelData(chapter, chapter.levels[i], (i + 1));
            }
        }
    }

    // pass in a LevelInfo and save it's data into the playerPrefs
    // call this when a level is completed
    public void SaveLevelData(ChapterInfo chapter, LevelInfo levelData, int levelNumber)
    {
        // check if the player got more gems
        if (levelData.gems > PlayerPrefs.GetInt($"Chapter_{chapter.chapterNum}_Level_{levelNumber}_Gems"))
        {
            // save the data 
            SaveData(chapter, levelData, levelNumber);
            // save the updated chapter info
            //            SaveChapterData(ChapterInfo chapterData, int chapterNumber);
        }
        // check if the player had the same amount of gems but a better time
        else if ((levelData.gems == PlayerPrefs.GetInt($"Chapter_{chapter.chapterNum}_Level_{levelNumber}_Gems")) && (levelData.time < PlayerPrefs.GetFloat($"Chapter_{chapter.chapterNum}_Level_{levelNumber}_Time")))
        {
            SaveData(chapter, levelData, levelNumber);
            //           SaveChapterData(ChapterInfo chapterData, int chapterNumber)
        }

        // save them right away
        PlayerPrefs.Save();
    }

    // save the data pieces for the level
    public void SaveData(ChapterInfo chapter, LevelInfo levelData, int levelNumber)
    {
        // Use keys with the level number as part of the key name
        PlayerPrefs.SetInt($"Chapter_{chapter.chapterNum}_Level_{levelNumber}_Stars", levelData.stars);
        PlayerPrefs.SetInt($"Chapter_{chapter.chapterNum}_Level_{levelNumber}_Score", levelData.score);
        PlayerPrefs.SetInt($"Chapter_{chapter.chapterNum}_Level_{levelNumber}_Gems", levelData.gems);
        PlayerPrefs.SetFloat($"Chapter_{chapter.chapterNum}_Level_{levelNumber}_Time", levelData.time);
        PlayerPrefs.SetInt($"Chapter_{chapter.chapterNum}_Level_{levelNumber}_Locked", levelData.locked ? 1 : 0);
    }

    // call this when a level is completed
    public void SaveLevelLock(ChapterInfo chapter, bool locked, int levelNumber)
    {
        // Use keys with the level number as part of the key name
        PlayerPrefs.SetInt($"Chapter_{chapter.chapterNum}_Level_{levelNumber}_Locked", locked ? 1 : 0);

        // save them right away
        PlayerPrefs.Save();
    }

    // pass in a levelNumber and load that levels data into levels[]
    // called when the game is loaded
    public void LoadAllLevelData(ChapterInfo chapter)
    {
        // loop through all the levels[] and load their data
        for (int i = 0; i < NUM_OF_LEVELS; i++)
        {
            // check if a level is locked if it is it still has default data
            if (PlayerPrefs.GetInt($"Chapter_{chapter.chapterNum}_Level_{i + 1}_Locked", 1) == 1)
            {
                break;
            }
            chapter.levels[i].gems = PlayerPrefs.GetInt($"Chapter_{chapter.chapterNum}_Level_{i + 1}_Gems", 0);
            chapter.levels[i].stars = PlayerPrefs.GetInt($"Chapter_{chapter.chapterNum}_Level_{i + 1}_Stars", 0);
            chapter.levels[i].score = PlayerPrefs.GetInt($"Chapter_{chapter.chapterNum}_Level_{i + 1}_Score", 0);
            chapter.levels[i].time = PlayerPrefs.GetFloat($"Chapter_{chapter.chapterNum}_Level_{i + 1}_Time", 0f);
            chapter.levels[i].locked = PlayerPrefs.GetInt($"Chapter_{chapter.chapterNum}_Level_{i + 1}_Locked", 1) == 1;
        }
    }

    #endregion

    #region Chapter Data

    // pass in a ChapterInfo and save it's data into the playerPrefs
    // should be called when a level is completed *************************
    public void SaveChapterData(ChapterInfo chapterData, int chapterNumber)
    {
        // Use keys with the level number as part of the key name
        // save the data for the chapter
        PlayerPrefs.SetInt($"Chapter_{chapterNumber}_Levels", chapterData.levelsCompleted);
        PlayerPrefs.SetInt($"Chapter_{chapterNumber}_Gems", chapterData.gemsCollected);
        PlayerPrefs.SetInt($"Chapter_{chapterNumber}_Percent", chapterData.percentCompleted);
        PlayerPrefs.SetInt($"Chapter_{chapterNumber}_Locked", chapterData.locked ? 1 : 0);
        // save them right away
        PlayerPrefs.Save();
    }

    // load chapter data for the given chapter
    // puts the chapter data from the PlayerData PlayerPrefs into a newly created ChapterInfo object
    public ChapterInfo LoadChapterData(int chapterNum)
    {
        // chapterInfo to return
        ChapterInfo loadedChapter = new ChapterInfo();
        // initalize the levels[]
        loadedChapter.InitalizeLevelsArray();

        // give numbers to the chapters
        loadedChapter.chapterNum = chapterNum;

        // check if a chapter is locked, if so it still has default data
        if (PlayerPrefs.GetInt($"Chapter_{chapterNum}_Locked", 1) == 1)
        {
            return loadedChapter;
        }

        // load in the other chapter data
        loadedChapter.levelsCompleted = PlayerPrefs.GetInt($"Chapter_{chapterNum}_Levels", 0);
        loadedChapter.gemsCollected = PlayerPrefs.GetInt($"Chapter_{chapterNum}_Gems", 0);
        loadedChapter.percentCompleted = PlayerPrefs.GetInt($"Chapter_{chapterNum}_Percent", 0);
        loadedChapter.locked = PlayerPrefs.GetInt($"Chapter_{chapterNum}_Locked", 1) == 1;

        // load the level data for this chapter till the loadedChapter's levels[] is full
        LoadAllLevelData(loadedChapter);

        return loadedChapter;
    }

    // load the chapter data for all chapters into the chapters array
    public void SetChaptersArray()
    {
        // loop through all the chapters[] and load their data
        for (int i = 0; i < LevelSelectManager.CHAPTER_COUNT; i++)
        {

            // check if a chapter is locked if it is it still has default data
            if (PlayerPrefs.GetInt($"Chapter_{i + 1}_Locked", 1) == 1)
            {
                break;
            }

            LevelSelectManager.instance.chapters[i] = LoadChapterData(i + 1);
        }
    }

    // call this when all levels in a chapter are completed ********************
    public void SaveChapterLock(bool locked, int chapterNumber)
    {
        // Use keys with the level number as part of the key name
        PlayerPrefs.SetInt($"Chapter_{chapterNumber}_Locked", locked ? 1 : 0);

        // save them right away
        PlayerPrefs.Save();
    }
    
    #endregion
}