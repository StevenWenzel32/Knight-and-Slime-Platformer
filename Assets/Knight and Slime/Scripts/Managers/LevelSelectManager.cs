using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

// handles the UI, transitions, and related functions for the level select menu
// displays the chapter info, handles transitions between chapters, handles starting levels
public class LevelSelectManager : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static LevelSelectManager instance {get; private set;}

    // consts
    // chapters will always have 10 levels
    const int NUM_OF_LEVELS = 10;

    [Header ("Start Up Info")]
    // the location of the chapters folder
    // to help find the chapter page objects to display -- kind of like giving it the right folder to look in
    public Transform chapters;

    [Header ("Page Controls")]
    public Transform rightArrow;
    public Transform leftArrow;

    // chapter completion progress
    [Header ("Chapter Display Info")]
    public TMP_Text chapterNum;
    public TMP_Text levelsCompleted;
    public TMP_Text totalGemsCollected;
    public TMP_Text percentCompleted;

    // chapter managment stuff
    // chapter info to be set to the one being saved
    // when a level is completed and the 
    public ChapterInfo chapter = new ChapterInfo ();
    // the current chapter / level select page 
    private int pageNum = 1;
    // to know when to have the page arrows disapear
    private int maxPageNum = 1;
    // to help switch which chapter is being displayed
    // the objects name is used for searching and other functions
    // acts a little bit like the page being displayed
    private Transform currentChapter;
    // helps with switching chapters
    private Transform lastChapter;

    // level loading and pop up stuff
    // the current level being looked at 
    private Transform level;
    // the lock of the current level
    private Transform levelLock;
    // the # of the next level after the current level
    private int nextLevelNumber;
    
    private void OnEnable(){
        
    }

    private void Awake(){
        if (instance == null)
        {
            instance = this;
            // Persist levelManager across scenes -- good for performance
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);
        }
        // set the current chapter to the start chapter
        currentChapter = ChapterManager.instance.startChapter;
        // setup the chapter page
        DisplayChapter(pageNum);
        // make sure the arrows are displayed correctly 
        ToggleLeftArrow();
        ToggleRightArrow();
    }

    // sends the player to the designated scene
    public void LoadLevel(int levelNumber)
    {
        // loads the given scene
        SceneManager.LoadScene(levelNumber + 2);
    }

    // the level buttons will call this function when clicked
    public void SelectLevel(int levelNumber){
        // find the level UI object
        level = currentChapter?.Find("Level " + levelNumber);
        // find it's lock
        levelLock = level?.Find("Level Lock");

        // check if the current level is unlocked
        if (!levelLock.gameObject.activeSelf){
            // set the next level number - not needed for now
            nextLevelNumber = levelNumber + 1;
            // load the level 
            LoadLevel(levelNumber);
        }
    }

    // do later ******
    // reflect the players score and gems collected using visual stars on the chapter page
    // is not called if level is failed
    public void UpdateStars(int stars){
        if (stars == 3){
            // show 3 stars on map
        } else if (stars == 2){
            // show 2 stars on map
        } else{
            // show 1 star on map
        }
    }

    // loop through the levels[] and reflect the lock data on the level buttons -- will show stars in future update ***
    public void SetUpLevels(){
        Debug.Log("set up levels is called");
        // loop through levels
        for (int i = 0; i < NUM_OF_LEVELS; i++)
        {
            // get the level buttons parent 
            Transform levelButton = currentChapter?.Find("Level " + (i + 1));
            // if the parent exists set the on click function
            if (levelButton != null){
                // get the button child and it's button component
                UnityEngine.UI.Button button = levelButton?.Find("Button")?.GetComponent<UnityEngine.UI.Button>();
                // if the button component exists
                if (button != null){
                    int levelIndex = i + 1;
                    // remove any listeners to be safe
                    button.onClick.RemoveAllListeners();
                    // add the correct listener with the correct function
                    button.onClick.AddListener(() => SelectLevel(levelIndex));
                    Debug.Log("Level Button Corrected: " + (i + 1));
                }
            }
            Debug.Log("LevelInfo for level: " + (i + 1) + ", Lock Status = " + SaveManager.instance.levels[i].locked);
            
            // check if the level is unlocked
            if (!chapter.levels[i].locked){
                // find the levelButton and make sure the lock is off
                currentChapter?.Find("Level " + (i + 1))?.Find("Level Lock").gameObject.SetActive(false);
                Debug.Log("Unlocked level: " + (i + 1));
            }
            // check if the level has any stars
            else if (SaveManager.instance.levels[i].stars != 0){
                // call the updateStars
                UpdateStars(SaveManager.instance.levels[i].stars);
                Debug.Log("Stars updated on level: " + (i + 1));
            }
            // if the level has no stars and is locked end the loop -- none of the rest have data
            else {
                break;
            }
        }
    }

    // get the chapter completion info from the save file and display it
    // should get called each time a level is completed *****
    public void DisplayChapterCompletionInfo(int pageNum){
        Debug.Log("displaying chapter info for chapter: " + pageNum);
        // display the title of pop up
        chapterNum.text = "Chapter " + pageNum;
        // get the number of levels passed
        levelsCompleted.text = chapter.SumCompletedLevels() + "/10";
        // get the total # of gems collected 
        totalGemsCollected.text = chapter.SumGemsCollected() + "/40";
        // show the percent of the chapter completed
        percentCompleted.text = chapter.CalculatePercent() + "%";
    }

    // based on the page number change the level select chapter to display 
    // calls the funcs to setup the chapter info and the levels
    // should get called each time the chapter page changes
    public void DisplayChapter(int pageNum){
        Debug.Log("displaying chapter: " + pageNum);
        // set last chapter to current chapter
        lastChapter = currentChapter;
        // find the chapter with the same number as the current page
        currentChapter = chapters?.Find("Chapter " + pageNum);
        // turn on this chapters map 
        currentChapter.gameObject.SetActive(true);
        // check if the last chapter is the current chapter
        if (lastChapter != currentChapter){
            // turn off the old chapters map
            lastChapter.gameObject.SetActive(false);
        }
        DisplayChapterCompletionInfo(pageNum);
        SetUpLevels();
    }

    // increase the page num and display the approriate chapter
    public void ChangePageRight(){
        // check if the page is at max
        if (pageNum != maxPageNum){
            pageNum++;
        }
        // call display chapter func
        DisplayChapter(pageNum);
        ToggleRightArrow();
    }

    // decrease the page num and display the approriate chapter
    public void ChangePageLeft(){
        // check if the page is at max
        if (pageNum != 1){
            pageNum--;
        }
        // call display chapter func
        DisplayChapter(pageNum);
        ToggleLeftArrow();
    }

    // toggles the right arrow on and off if needed
    private void ToggleRightArrow(){
        // check if page num is max
        if (pageNum == maxPageNum){
            rightArrow.gameObject.SetActive(false);
        } else {
            rightArrow.gameObject.SetActive(true);
        }
    }

    // toggles the left arrow on and off if needed
    private void ToggleLeftArrow(){
        // check if page num is max
        if (pageNum == 1){
            leftArrow.gameObject.SetActive(false);
        } else {
            leftArrow.gameObject.SetActive(true);
        }
    }

// --- for saving chapter data ---
    // level is completed
    // level data is saved
    // chapter data is updated/saved

// --- for loading level data ---
    // put the levels array inside the chapterInfo
    // levels should be loaded after the chapter page has been loaded 
}
