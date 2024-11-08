using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static LevelSelectManager instance {get; private set;}
    public Transform levelsUICanvas;

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
        SetUpLevels();
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
        level = levelsUICanvas?.Find("Level " + levelNumber);
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

    // finish later -- not needed for class 
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
        for (int i = 0; i < SaveManager.instance.levels.Length; i++)
        {
            // get the level buttons parent 
            Transform levelButton = levelsUICanvas?.Find("Level " + (i + 1));
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
            Debug.LogError("LevelInfo for level: " + (i + 1) + ", Lock Status = " + SaveManager.instance.levels[i].locked);
            // check if the level has any stars
            if (SaveManager.instance.levels[i].stars != 0){
                // call the updateStars
                UpdateStars(SaveManager.instance.levels[i].stars);
                Debug.Log("Stars updated on level: " + (i + 1));
            }
            // check if the level is unlocked
            else if (!SaveManager.instance.levels[i].locked){
                // find the levelButton and make sure the lock is off
                levelsUICanvas?.Find("Level " + (i + 1))?.Find("Level Lock").gameObject.SetActive(false);
                Debug.Log("Unlocked level: " + (i + 1));
            }
            // if the level has no stars and is locked end the loop -- none of the rest have data
            else {
                break;
            }
        }
    }
}
