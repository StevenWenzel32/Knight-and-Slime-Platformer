using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class LevelSelectManager : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static LevelSelectManager instance {get; private set;}

    // private vars
    private Transform level;
    private Transform levelLock;
    private int nextLevelNumber;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    // the level buttons will call this function -- will never be fed level 1
    public void SelectLevel(int levelNumber){
        // find the level UI object
        level = transform?.Find("Level " + levelNumber);
        // find it's lock
        levelLock = level?.Find("Level Lock");
        // set the next level number
        nextLevelNumber = levelNumber++;

        // check if the current level is unlocked
        if (!levelLock.gameObject.activeSelf){
            // load the level 
            MenuManager.instance.LoadLevel(levelNumber);
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

    // removes the lock of the next level -- not needed yet
    public void RemoveLock(int nextLevelNumber){
        Transform nextLevelLock = transform?.Find("Level " + nextLevelNumber)?.Find("Level Lock");
        if (nextLevelLock.gameObject.activeSelf){
            // turn the lock off on the level select menu
            nextLevelLock.gameObject?.SetActive(false);
            // turn off the lock on the LevelInfo mess with later
        //   LevelManager.instance.levelInfo.locked
        }
    }

    // display level info when hovered over
    public void DisplayLevelInfo(){

    }
}
