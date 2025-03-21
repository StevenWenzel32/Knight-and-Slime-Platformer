using UnityEngine;

// using a class instead of a struct because it is better on the memory when used inside of large arrays
// in the future will want to add more chapters and it's unknown how many there will be 
// handles the stuff that affects only 1 chapter, one chapters data or one chpaters functions
public class ChapterInfo{
    public int chapterNum = 0;
    public int levelsCompleted = 0;
    public int gemsCollected = 0;
    public int percentCompleted = 0;
    // chapters will be locked by default, previous chapter must be completed before unlocking the next one
    public bool locked = true;
    // the bookmark state used to toggle the visuals of the bookmark icon for the chapter
    public bool bookmarked = false;
    // for initalizing size of levels[]
    public int levelCount = 10;
    // the data for the 10 levels of this chapter
    public LevelInfo[] levels;

    // helper functions
    // create the array to store the level data and load in the saved data
    // pass in the current chapter
    public void InitalizeLevelsArray(){
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
    }

    // toggle the chapters lock state
    public void ToggleLock(){
        locked = !locked;
    }

    // toggle the chapters bookmark state
    public void ToggleBookmark(){
        bookmarked = !bookmarked;
    }

    // getters and setters
}
