using TMPro;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Data;
using Unity.VersionControl.Git.IO;

// one for the whole game
// is the middle man between SaveManager and LevelSelectManager
// keeps track of over-all game progress across all chapters
// keeps track of chapter unlock progress
// keeps track of the current chapter
public class ChapterManager : MonoBehaviour
{
    // only have one instance ever in the scene, can get else where but can only set here -- singleton -- if you use don't destroy on load it will last across scenes
    public static ChapterManager instance {get; private set;}
    // for initalizing chapters[] size -- need to change the array's name to chapterBuffer[]
    // change back to a size of 3 once there are 3 or more chapters
    // 1 is a place holder 
    const int CHAPTER_BUFFER_COUNT = 1;
    // The number of chapters currently in the game
    const int CHAPTER_COUNT = 1;
    // where the saved chapter data gets loaded and where new data goes
    // chapters are stored in this order: current, last, next
    public ChapterInfo[] chapters;
    // what chapter to display on start -- give players the option to bookmark a chapter which will set this
    // the chapter that starts as the current chapter on start up
    public Transform startChapter;
    // keeps track of the chapter object that is currently being looked at and used
    public ChapterInfo currentChapterInfo;
    // used to keep track of the current chapter #
    public int currentChapterNumber;
    // the offest used to keep track of the current levels location in the chapters[], is wrapping
    // the current chapter starts at chapters[0], add the offset to this
    // off set can be one of these values: 0, 1, 2
    // aka the current chapters location
    private int currentChapterOffset = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        // singleton stuff
        if (instance == null){
            instance = this;
            // Persist chapterManager across scenes -- good for performance
            DontDestroyOnLoad(gameObject);

            // initialize the chapters manager
            InitalizeChaptersArray();
            // load the default or the bookmarked chapter 
            LoadChapterIntoBuffer(1,0);
        } else {
            Destroy(gameObject);
        }
    }

    // load the previous chapter -- called when moving a page to the left
    // not needed until there are more than 10 levels ************
    public void LoadPreviousChapter(){
        
    }

    // load the next chapter -- called when moving a page to the right
    // handles all the chapters[] buffer related operations
    // not needed until there are more than 10 levels ************
    public void LoadNextChapter(){
        // update currentChapterNum
        currentChapterNumber++;
        // update offset
        IncreaseChapterOffset();
        // unload previous chapter
//        UnloadChapterFromBuffer(chapterNum, index);
        // load next chapter
//        LoadChapterIntoBuffer(chapterNum, index);
    }

    // changes the chapter being shown. calls helper functions that handle sepcific parts of the process
    // changes based on if they are moving to the next or previous chapter
    // true = show the next chapter
    // false = show the previous chapter
    // not needed until there are more than 10 levels ************
    private void ChangeChapter(bool nextChapter){
        if (nextChapter){
            LoadNextChapter();
        } else {
            LoadPreviousChapter();
        }
        UpdateChapterInfo();
    }

    // loads the given chapter into the chapters[] at the given index
    // uses the save manager to load the chapter data
    public void LoadChapterIntoBuffer(int chapterNum, int index){ 
        // load new chapter over chapters[index]
        chapters[index] = SaveManager.instance.LoadChapterData(chapterNum);
    }

    // unloads the given chapter and resaves its data to the save files -- currently player Prefs
    // not needed until there are more than 10 levels ************
    public void UnloadChapterFromBuffer(int chapterNum, int index){
        // save chapter data

        // remove from chapters[] -- set the given location to default values -- might not be needed
    }

    // called when progressing to higher # chapters/ going to the right in the UI
    // not needed until there are more than 10 levels ************
    public void IncreaseChapterOffset(){
        // check what the value of the offset already is
    }

    // called when decreasing to lower # chapters/ going to the left in the UI
    // not needed until there are more than 10 levels ************
    public void DecreaseChapterOffset(){

    }

    // initalize the chapters array
    private void InitalizeChaptersArray(){
        // initalize chapters[]
        chapters = new ChapterInfo[CHAPTER_BUFFER_COUNT];
        // since the chapterInfo is a class not a struct need to initalize the array before use
        for (int i = 0; i < CHAPTER_BUFFER_COUNT; i++)
        {
            chapters[i] = new ChapterInfo();
            // if the first chapter make sure it's unlocked
            if (i == 0){
                chapters[i].locked = false;
            }
        }
    }

    // update the current chapterInfo that is being displayed
    private void UpdateChapterInfo(){
        
    }

    // save the new chapter data -- should be saved when the level data is saved ******
    private void SaveNewChapterData(){
        Debug.Log("Chapter data changed");

    }

    // unlock the next chapter if it isn't already unlocked
    // not needed until there are more than 10 levels ************
    private void UnlockNextChapter(){

    }
}
