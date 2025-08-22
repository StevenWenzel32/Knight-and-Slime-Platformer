using TMPro;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Data;
using Unity.VersionControl.Git.IO;

// one for the whole game
// kind of a cross between levelmanager and levelSelectManager but for chapters
    // it controls the selection and display of the chapters (like levelSelectManager) but also the function and data of the chapters (like LevelManager)
// is the middle man between SaveManager and LevelSelectManager
// keeps track of over-all game progress across all chapters
// keeps track of chapter unlock progress
// keeps track of the current chapter
public class ChapterManager : MonoBehaviour
{
    // only have one instance ever in the scene, can get else where but can only set here -- singleton -- if you use don't destroy on load it will last across scenes
    public static ChapterManager instance { get; private set; }
    // number of chapters in the game, used for initalizing chapters[] size 
    public const int CHAPTER_COUNT = 1;
    // where the saved chapter data gets loaded and where new data goes
    public ChapterInfo[] chapters;
    // what chapter to display on start -- give players the option to bookmark a chapter which will set this
    // the chapter that starts as the current chapter on start up
    public Transform startChapter;
    // keeps track of the chapter object that is currently being looked at and used
    public ChapterInfo currentChapterInfo;
    // used to keep track of the current chapter #
    public int currentChapterNumber;

    // Start is called before the first frame update
    void Awake()
    {
        // singleton stuff
        if (instance == null)
        {
            instance = this;
            // Persist chapterManager across scenes -- good for performance
            DontDestroyOnLoad(gameObject);

            // initialize the chapters manager
            InitalizeChaptersArray();
            // load all the chapters
            SaveManager.instance.SetChaptersArray();

            // load the default/first chapter or the bookmarked chapter 
            // will have to change the chapternum passed here to a varibale later that is connected to which chapter is bookmarked
            LevelSelectManager.instance.DisplayChapter(1);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // initalize the chapters array
    private void InitalizeChaptersArray()
    {
        // initalize chapters[]
        chapters = new ChapterInfo[CHAPTER_COUNT];
        // since the chapterInfo is a class not a struct need to initalize the array before use
        for (int i = 0; i < CHAPTER_COUNT; i++)
        {
            chapters[i] = new ChapterInfo();
            // if the first chapter make sure it's unlocked
            if (i == 0)
            {
                chapters[i].locked = false;
            }
        }
    }

    // save the new chapter data -- should be saved when the level data is saved ******
    private void SaveNewChapterData()
    {
        Debug.Log("Chapter data changed");

    }

    // unlock the next chapter if it isn't already unlocked
    // not needed until there are more than 10 levels ************
    private void UnlockNextChapter()
    {
        Debug.Log("Next chapter unlocked");

    }
}
