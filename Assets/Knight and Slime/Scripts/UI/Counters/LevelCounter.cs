using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCounter : MonoBehaviour
{
    // only have one instance ever in the scene, can get else where but can only set here -- singleton -- if you use don't destroy on load it will last across scenes
    public static LevelCounter instance {get; private set;}

    private TMP_Text text;
    public int levelNumber;
    private void Awake()
    {
        // singleton stuff
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }

        text = GetComponent<TMP_Text>();
        // set the levelSceneName
        string levelSceneName = SceneManager.GetActiveScene().name;
        // set the levelNumber in the levelManager
        levelNumber = ParseName(levelSceneName);
        text.text = "Level " + levelNumber;
    }

    public int ParseName(string levelSceneName){
        string[] splitName = levelSceneName.Split(" ");
        return int.Parse(splitName[1]);
    }
}

