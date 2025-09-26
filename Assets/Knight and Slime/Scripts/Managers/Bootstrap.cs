using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    void Awake()
    {
        // Load the main menu
        SceneManager.LoadScene("Main Menu");
    }
}
