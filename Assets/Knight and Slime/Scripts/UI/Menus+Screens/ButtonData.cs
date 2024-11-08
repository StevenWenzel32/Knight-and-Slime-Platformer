using UnityEngine;

// a class to hold button info based on it's components and pass that info into functions that need it
// to be used for feeding info into a popup
public class ButtonData : MonoBehaviour
{ 
    // feeds the levelNum for pop up to retrieve levelInfo
    public int levelNum;

    public void Awake(){
        // fill in the data 
        levelNum = ParseName(gameObject.name);
    }

    // show pop up
    public void OnHover(){
        PopUpControl.instance.ShowPopUp(this);
    }

    // hide pop up
    public void OnExit(){
        PopUpControl.instance.HidePopUp();
    }

    // get the number from the object name
    public int ParseName(string objectName){
        string[] splitName = objectName.Split(" ");
        return int.Parse(splitName[1]);
    }
}
