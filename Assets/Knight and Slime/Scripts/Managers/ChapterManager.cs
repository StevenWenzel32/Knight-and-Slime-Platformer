using TMPro;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

// one for the whole game
// is the middle man between SaveManager and LevelSelectManager
// keeps track of over-all game progress across all chapters
// keeps track of chapter unlock progress
// keeps track of the current chapter
public class ChapterManager : MonoBehaviour
{
    // only have one instance ever in the scene, can get else where but can only set here -- singleton -- if you use don't destroy on load it will last across scenes
    public static ChapterManager instance {get; private set;}
    // for initalizing chapters[] size
    private int chapterCount = 3;
    // where the saved chapter data gets loaded and where new data goes
    // chapters are stored in this order: current, last, next
    public ChapterInfo[] chapters;
    // what chapter to display on start -- give players the option to bookmark a chapter which will set this
    // the chapter that starts as the current chapter on start up
    public Transform startChapter;
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
            // Persist levelManager across scenes -- good for performance
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        
    }

    // load the previous chapter -- called when moving a page to the left
    public void LoadPreviousChapter(){
        
    }

    // load the next chapter -- called when moving a page to the right
    public void LoadNextChapter(){
        // update currentChapterNum
        currentChapterNumber++;
        // update offset
        IncreaseChapterOffset();
        // unload previous chapter
        UnloadChapterFromBuffer(chapterNum, index);
        // load next chapter
        LoadChapterIntoBuffer(chapterNum, index);
    }

    // loads the given chapter into the chapters[]
    public void LoadChapterIntoBuffer(int chapterNum, int index){
        // load new chapter over chapters[index]

    }

    // unloads the given chapter and resaves its data to the save files -- currently player Prefs
    public void UnloadChapterFromBuffer(int chapterNum, int index){
        // save chapter data

        // remove from chapters[] -- set the given location to default values -- might not be needed
    }

    // called when progressing to higher # chapters/ going to the right in the UI
    public void IncreaseChapterOffset(){
        // check what the value of the offset already is
    }

    // called when decreasing to lower # chapters/ going to the left in the UI
    public void DecreaseChapterOffset(){

    }

    // initalize the chapters array -- move loading the data into it somewhere else *****
    private void InitalizeChaptersArray(){
        // initalize chapters[]
        chapters = new ChapterInfo[chapterCount];
        // since the chapterInfo is a class not a struct need to initalize the array before use
        for (int i = 0; i < chapterCount; i++)
        {
            chapters[i] = new ChapterInfo();
            // if the first level make sure it's unlocked
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

    // unlock the next level if it isn't already unlocked
    private void UnlockNextChapter(){

    }
}
