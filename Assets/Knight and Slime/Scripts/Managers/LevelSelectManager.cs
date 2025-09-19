using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

// handles the UI, transitions, and related functions for the level select menu
// handles all the chapter related functions aside from saving and loading which is in the SaveManager
// it controls the selection and display of the chapters and also the function and data of the chapters (like LevelManager)
// keeps track of over-all game progress across all chapters
// keeps track of chapter unlock progress
// keeps track of the current chapter
// displays the chapter info, handles transitions between chapters, handles starting levels
public class LevelSelectManager : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static LevelSelectManager instance { get; private set; }

    // consts
    // number of chapters in the game, used for initalizing chapters[] size 
    public const int CHAPTER_COUNT = 1;

    [Header("Start Up Info")]
    // the location of the chapters folder
    // to help find the chapter page objects to display -- kind of like giving it the right folder to look in
    private Transform chaptersParent;

    [Header("Page Controls")]
    private Transform rightArrow;
    private Transform leftArrow;

    // chapter completion progress
    [Header("Chapter Display Info")]
    // where to display the chapters info
    private TMP_Text chapterNum;
    private TMP_Text levelsCompleted;
    private TMP_Text totalGemsCollected;
    private TMP_Text percentCompleted;

    // chapter management stuff
    // where the saved chapter data gets loaded and where new data goes
    public ChapterInfo[] chapters;
    // what chapter to display on start -- give players the option to bookmark a chapter which will set this *********
    // the chapter that starts as the current chapter on start up
    public int startChapterNum = 1;
    // the chapter that is currently accessed and is being updated 
    public ChapterInfo currentChapter = new ChapterInfo();
    // the current chapter / level select page 
    private int currentPageNum;
    // to know when to have the page arrows disapear
    private int maxPageNum = 1;
    // to help switch which chapter is being displayed
    // the objects name is used for searching and other functions
    // acts a little bit like the page being displayed
    private Transform currentChapterPage;
    // helps with switching chapters
    private Transform lastChapterPage;

    // level loading and pop up stuff
    // the current level being looked at 
    private Transform level;
    // the lock of the current level
    private Transform levelLock;
    // the # of the next level after the current level
    private int nextLevelNumber;

    // the stuff that needs to happen each time the LevelMenu is reloaded/enabled
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("LevelSelectManager On Enable - finished running");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level Select Menu")
        {
            // get the specifc scene transforms and other references
            LoadSceneReferences();
            // Refresh the current chapter display
            currentPageNum = currentChapter.chapterNum;
            DisplayChapter(currentPageNum);
        }
    }

    private void Awake()
    {
        // stuff that happens once when the object is created
        if (instance == null)
        {
            instance = this;
            // persist across scenes to improve performance
            // keeps chapters[] alive
            DontDestroyOnLoad(gameObject);

            // initialize the chapters array
            InitalizeChaptersArray();
            // load all the chapters
            SaveManager.instance.SetChaptersArray();

            // select the first chapter
            currentChapter = chapters[startChapterNum - 1];
            // get the currentPageNum 
            currentPageNum = currentChapter.chapterNum;
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log("LevelSelectManager Awake - finished running");
    }

    #region level selection

    // sends the player to the designated scene
    // will need to be changed later to account for chapters ***********
    public void LoadLevel(int levelNumber)
    {
        // loads the given scene
        SceneManager.LoadScene("Level " + levelNumber);
    }

    // this gets called via the function in BasicButtons
    public void SelectLevel(int levelNumber)
    {
        // find the level UI object
        level = currentChapterPage?.Find("Level " + levelNumber);
        // find it's lock
        levelLock = level?.Find("Level Lock");

        // check if the current level is unlocked
        if (!levelLock.gameObject.activeSelf)
        {
            // set the next level number - not needed for now
            nextLevelNumber = levelNumber + 1;
            // load the level 
            LoadLevel(levelNumber);
        }
    }
    #endregion

    #region level display
    // do later ******
    // reflect the players score and gems collected using visual stars on the chapter page
    // is not called if level is failed
    public void UpdateStars(int stars)
    {
        if (stars == 3)
        {
            // show 3 stars on map
        }
        else if (stars == 2)
        {
            // show 2 stars on map
        }
        else
        {
            // show 1 star on map
        }
    }

    // loop through the levels[] and reflect the lock data on the level buttons -- will show stars in future update ***
    public void SetUpLevels()
    {
        Debug.Log("set up levels is called");
        // loop through levels
        for (int i = 0; i < ChapterInfo.LEVEL_COUNT; i++)
        {
            // get the level buttons parent 
            Transform levelButton = currentChapterPage?.Find("Level " + (i + 1));
            // if the parent exists set the on click function
            if (levelButton != null)
            {
                // get the button child and it's button component
                UnityEngine.UI.Button button = levelButton?.Find("Button")?.GetComponent<UnityEngine.UI.Button>();
                // if the button component exists
                if (button != null)
                {
                    int levelIndex = i + 1;
                    // remove any listeners to be safe
                    button.onClick.RemoveAllListeners();
                    // add the correct listener with the correct function
                    button.onClick.AddListener(() => SelectLevel(levelIndex));
                    Debug.Log("Level Button Corrected: " + (i + 1));
                }
            }
            Debug.Log("LevelInfo for level: " + (i + 1) + ", Lock Status = " + currentChapter.levels[i].locked);

            // check if the level is unlocked
            if (!currentChapter.levels[i].locked)
            {
                // find the levelButton and make sure the lock is off
                currentChapterPage?.Find("Level " + (i + 1))?.Find("Level Lock").gameObject.SetActive(false);
                Debug.Log("Unlocked level: " + (i + 1));
            }
            // check if the level has any stars
            else if (currentChapter.levels[i].stars != 0)
            {
                // call the updateStars
                UpdateStars(currentChapter.levels[i].stars);
                Debug.Log("Stars updated on level: " + (i + 1));
            }
            // if the level has no stars and is locked end the loop -- none of the rest have data
            else
            {
                break;
            }
        }
    }
    #endregion

    #region Chapter

    // load scene specific refrences
    private void LoadSceneReferences()
    {
        //reassign the scene specifc references
        chaptersParent = GameObject.Find("Level UI Canvas").transform.Find("Chapters")?.transform;
        Debug.Log("chaptersParent is: " + chaptersParent);
        chapterNum = GameObject.Find("ChapterNumText")?.GetComponent<TMP_Text>();
        levelsCompleted = GameObject.Find("LevelNumText")?.GetComponent<TMP_Text>();
        totalGemsCollected = GameObject.Find("GemNumText")?.GetComponent<TMP_Text>();
        percentCompleted = GameObject.Find("PercentText")?.GetComponent<TMP_Text>();
        rightArrow = GameObject.Find("Right Arrow")?.transform;
        leftArrow = GameObject.Find("Left Arrow")?.transform;

        // make sure the arrows are displayed correctly 
        ToggleLeftArrow();
        ToggleRightArrow();
    }

    // initalize the chapters array
    private void InitalizeChaptersArray()
    {
        // initalize chapters[]
        instance.chapters = new ChapterInfo[CHAPTER_COUNT];
        // since the chapterInfo is a class not a struct need to initalize the array before use
        for (int i = 0; i < CHAPTER_COUNT; i++)
        {
            instance.chapters[i] = new ChapterInfo();
            // if the first chapter make sure it's unlocked
            if (i == 0)
            {
                instance.chapters[i].locked = false;
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

    // get the chapter completion info from the save file and display it
    // should get called each time a level is completed *****
    public void DisplayChapterCompletionInfo(int pageNum)
    {
        Debug.Log("displaying chapter info for chapter: " + pageNum);
        // display the title of pop up
        chapterNum.text = "Chapter " + pageNum;
        // get the number of levels passed
        levelsCompleted.text = currentChapter.SumCompletedLevels() + "/10";
        // get the total # of gems collected 
        totalGemsCollected.text = currentChapter.SumGemsCollected() + "/40";
        // show the percent of the chapter completed
        percentCompleted.text = currentChapter.CalculatePercent() + "%";
    }

    public void DisplayStartChapter(int pageNum)
    {
        Debug.Log("displaying first chapter: " + pageNum);
        // find the chapter with the same number as the current page
        currentChapterPage = chaptersParent?.Find("Chapter " + pageNum);
        Debug.Log("Current Chapter page is = " + currentChapterPage);
        // turn on this chapters map 
        currentChapterPage.gameObject.SetActive(true);
        DisplayChapterCompletionInfo(pageNum);
        SetUpLevels();
    }

    // based on the page number change the level select chapter to display 
    // calls the funcs to setup the chapter info and the levels
    // should get called each time the chapter page changes
    public void DisplayChapter(int pageNum)
    {
        Debug.Log("displaying chapter: " + pageNum);
        // set last chapter to current chapter
 //       lastChapterPage = currentChapterPage;
        // find the chapter with the same number as the current page
        currentChapterPage = chaptersParent?.Find("Chapter " + pageNum);
        // turn on this chapters map 
        currentChapterPage.gameObject?.SetActive(true);
        // check if the last chapter is the current chapter
//        if (lastChapterPage != currentChapterPage)
 //       {
          // turn off the old chapters map
  //          lastChapterPage.gameObject.SetActive(false);
  //      }
        DisplayChapterCompletionInfo(pageNum);
        SetUpLevels();
    }

    // increase the page num and display the approriate chapter
    public void ChangePageRight()
    {
        // check if the page is at max
        if (currentPageNum != maxPageNum)
        {
            currentPageNum++;
        }
        // call display chapter func
        DisplayChapter(currentPageNum);
        ToggleRightArrow();
    }

    // decrease the page num and display the approriate chapter
    public void ChangePageLeft()
    {
        // check if the page is at max
        if (currentPageNum != 1)
        {
            currentPageNum--;
        }
        // call display chapter func
        DisplayChapter(currentPageNum);
        ToggleLeftArrow();
    }

    // toggles the right arrow on and off if needed
    private void ToggleRightArrow()
    {
        // check if page num is max
        if (currentPageNum == maxPageNum)
        {
            rightArrow.gameObject.SetActive(false);
        }
        else
        {
            rightArrow.gameObject.SetActive(true);
        }
    }

    // toggles the left arrow on and off if needed
    private void ToggleLeftArrow()
    {
        // check if page num is max
        if (currentPageNum == 1)
        {
            leftArrow.gameObject.SetActive(false);
        }
        else
        {
            leftArrow.gameObject.SetActive(true);
        }
    }
    #endregion
}