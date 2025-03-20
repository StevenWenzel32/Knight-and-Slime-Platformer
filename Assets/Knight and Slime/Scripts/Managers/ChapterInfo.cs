using UnityEngine;

// using a class instead of a struct because it is better on the memory when used inside of large arrays
// in the future will want to add more chapters and it's unknown how many there will be 
public class ChapterInfo{
    public int levelsCompleted = 0;
    public int gemsCollected = 0;
    public int percentCompleted = 0;
    // chapters will be locked by default, previous chapter must be completed before unlocking the next one
    public bool locked = true;
    // the data for the 10 levels of this chapter
    public LevelInfo[] levels;
}
