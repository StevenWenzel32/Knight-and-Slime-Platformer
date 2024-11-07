using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static LevelSelectManager instance {get; private set;}

    // the current level being looked at 
    private Transform level;
    // the lock of the current level
    private Transform levelLock;
    // the # of the next level after the current level
    private int nextLevelNumber;
    // perhaps find a way to set the text of the level objects automatically 

    private void Awake(){
        if (instance == null)
        {
            instance = this;
            // Persist SoundManager across scenes -- good for performance
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);
        }
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
        level = transform?.Find("Level " + levelNumber);
        // find it's lock
        levelLock = level?.Find("Level Lock");

        // check if the current level is unlocked
        if (!levelLock.gameObject.activeSelf){
            // set the next level number
            nextLevelNumber = levelNumber++;
            // load the level 
            LoadLevel(levelNumber);
        }
    }

    // finish later -- not needed for class 
    // is not called if level is failed
    public void UpdateStars(int stars){
        // if a star is earned calls removeLock for the next level
        if (stars == 3){
            // show 3 stars on map
        } else if (stars == 2){
            // show 2 stars on map
        } else{
            // show 1 star on map
        }
        RemoveLock(nextLevelNumber);
    }

    // removes the lock of the next level -- will never be fed level 1
    public void RemoveLock(int nextLevelNumber){
        // find the nextLevel and it's lock
        Transform nextLevelLock = transform?.Find("Level " + nextLevelNumber)?.Find("Level Lock");
        // check if the lock is active
        if (nextLevelLock.gameObject.activeSelf){
            // turn the lock off on the level select menu
            nextLevelLock.gameObject?.SetActive(false);
            // turn off the lock in the LevelInfo
           SaveManager.instance.levels[nextLevelNumber - 1].locked = false;
        }
    }

    // display level info when hovered over
    public void DisplayLevelInfo(){

    }
}
