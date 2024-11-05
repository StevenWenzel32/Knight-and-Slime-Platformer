using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// saves player data in binary form for security 
public class SaveManager : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static SaveManager instance {get; private set;}

    // what we want to save
    public LevelInfo[] levels;
    // for initalizing size
    private int levelCount = 10;

    public void Awake(){
        // singleton stuff
        if (instance == null){
            instance = this;
            // make sure this persists across scenes
            DontDestroyOnLoad(gameObject);
            // initalize the levels to a size
            levels = new LevelInfo[levelCount];
            // load in the saved PlayerData
            LoadAll();
        } else {
            Destroy(gameObject);
        }
    }

    // load all player data from the playerInfo.dat if it exists
    public void LoadAll(){
        // check if the file exsits
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")){
            // make the formatter
            BinaryFormatter bf = new BinaryFormatter();
            // open the file with player data
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            // deserialize the file so it can be read and converted into a PlayerData type
            PlayerData data = (PlayerData) bf.Deserialize(file);

            // loop through all the levels and load their data
            for (int i = 0; i < data.levels.Length; i++){
                // if the levelInfo has not be changed yet stop the copying
                if (data.levels[i].levelNumber == 0)
                {
                    break;
                }
                levels[i] = data.levels[i];
            }

            file.Close();
        }
    }

    // load just one level
    public void LoadLevel(int levelNumber){
        // check if the file exsits
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")){
            // make the formatter
            BinaryFormatter bf = new BinaryFormatter();
            // open the file with player data
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            // deserialize the file so it can be read and converted into a PlayerData type
            PlayerData data = (PlayerData) bf.Deserialize(file);

            // have the current levelOne be equal to the saved one
            levels[levelNumber - 1] = data.levels[levelNumber - 1];

            file.Close();
        }
    }

    // can only save primative data = save numbers, bools, strings, chars, and arrays
    // levelName should be in the format of levelOne, levelTwo, etc.
    // save all level info
    public void SaveAll(){
        // to format the data into binary for security 
        BinaryFormatter bf = new BinaryFormatter();
        // using perstitant data path so it will choose a directory dynamically 
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData data = new PlayerData();

        // loop through all the levels and save their data
        // stops at the first empty level
        for (int i = 0; i < levels.Length; i++){
            // if the levelInfo has not be changed yet stop the copying
            if (levels[i].levelNumber == 0)
            {
                break;
            }
            data.levels[i] = levels[i];
        }

        bf.Serialize(file, data);
        file.Close();
    }

    // can only save primative data = save numbers, bools, strings, chars, and arrays
    // levelName should be in the format of levelOne, levelTwo, etc.
    // save only one levelInfo top PlayerData
    public void SaveLevel(int levelNumber){
        // to format the data into binary for security 
        BinaryFormatter bf = new BinaryFormatter();
        // using perstitant data path so it will choose a directory dynamically 
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData data = new PlayerData();

        // have the saved levelOne be equal to the current one
        data.levels[levelNumber - 1] = levels[levelNumber - 1];

        bf.Serialize(file, data);
        file.Close();
    }
}

// level info class -- offers more flexability for seralizing than structs
// need to set the data in other scripts when the level ends -- inside levelManager
[Serializable]
public class LevelInfo{
    public int levelNumber = 0;
    public int gems = 0;
    public int stars = 0;
    public int score = 0;
    public float time = 0;
    // probably need this to let players access the proper levels
    public bool locked = true;
}

// look into why this is needed
// for changing the class into a different format
[Serializable]
class PlayerData{
    // array of levelInfo objects to save
    public LevelInfo[] levels = new LevelInfo[10];
    
}