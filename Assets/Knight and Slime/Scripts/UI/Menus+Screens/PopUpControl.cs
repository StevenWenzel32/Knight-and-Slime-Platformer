using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class PopUpControl : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static PopUpControl instance {get; private set;}

    [Header ("Popup Type")]
    public bool levelInfo;

    [Header ("Popup Panel")]
    public GameObject popUpPanel;
    public int popupMargin;
//  public GameObject button;

    [Header ("Popup Info")]
    public TMP_Text title;
    public TMP_Text gems;
    public TMP_Text time;
    public TMP_Text score;
    
    // private vars
    // the transform for the popup
    private RectTransform rectTransform;

    private void Awake()
    {
        // singleton stuff
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    } 

    public void Start(){
        if (popUpPanel == null)
        {
            Debug.LogError("Popup panel has not been assigned");
            return;
        }

        popUpPanel.SetActive(false);
        rectTransform = popUpPanel.GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("the rect transform for the popup panel is not found");
            return;
        }
    }

    // Update the position of the popup to follow the mouse -- most of this is broken while the positions of the objects in the scene are as they are 
    // unsure how to fix these things -- the buttons all somehow have the same position 
    // bottom left of the screen is not 0,0
    // problem for another time **
    void UpdatePopupPosition()
    {   // get mouse position
        Vector2 mousePos = Input.mousePosition;
        // adjust the y position of the pop up using the margin -- leave it at just this for now
        mousePos.y += popupMargin;
        // set the adjusted positon of the pop up 
        Vector2 adjustedPosition = mousePos;

        // // get the screen width 
        // float screenWidth = Screen.width;
        // // get the screen higeht 
        // float screenHeight = Screen.height;

        // // get the size of the pop up 
        // float popupWidth = rectTransform.rect.width;
        // float popupHeight = rectTransform.rect.height;

        // get the position and size of the button
        // Vector2 buttonPosition = button.GetComponent<RectTransform>().position;
        // Vector2 buttonSize = button.GetComponent<RectTransform>().rect.size;

        // float buttonTopMargin = buttonPosition.y - buttonSize.y;

        // if the position of the button plus the buttons height overlaps with the position of the popup 
            // give a margin of 

        // // checks to see if the pop up will be off the screen -- broken
        // // if off the right side
        // if (adjustedPosition.x + (popupWidth / 2) > (screenWidth / 2)){
        //     Debug.LogError("Popup was going off the right side");
        //     // then position it to the left
        //     adjustedPosition.x = (screenWidth / 2) - (popupWidth / 2);
        // } else if (adjustedPosition.x - (popupWidth / 2) < (-screenWidth / 2)){
        //     Debug.LogError("Popup was going off the left side");
        //     // then position it to the right
        //     adjustedPosition.x = (-screenWidth / 2) + (popupWidth / 2);
        // }

        // // if off the top 
        // if (adjustedPosition.y + (popupHeight / 2) > (screenHeight / 2)){
        //     Debug.LogError("Popup was going off the top side");
        //     // then position it to the bottom
        //     adjustedPosition.y = (screenHeight / 2) - (popupHeight / 2);
        // } else if (adjustedPosition.y - (popupHeight / 2) < (-screenHeight / 2)){
        //     Debug.LogError("Popup was going off the bottom side");
        //     // then position it to the top
        //     adjustedPosition.y = (-screenHeight / 2) + (popupHeight / 2);
        // }

        // set the position of the pop up using any changes made above
        rectTransform.position = adjustedPosition;
    }

    public void ShowPopUp(ButtonData data){
        // check if the level is unlocked
        if (!SaveManager.instance.levels[data.levelNum - 1].locked){
            FillPopup(data);
            UpdatePopupPosition();
            popUpPanel.SetActive(true);
        }
    }

    public void HidePopUp(){
        popUpPanel.SetActive(false);
    }

    // uses passed in data to fill in the pop up 
    public void FillPopup(ButtonData data){
        if (levelInfo){
            DisplayLevelInfo(data);
        }
    }

    public void DisplayLevelInfo(ButtonData data){
        // display the title of pop up
        title.text = "Level " + data.levelNum;
        // check if the level has been beaten 
        if (SaveManager.instance.levels[data.levelNum - 1].stars != 0){
             // just having the # of gems always be 4 for now
            gems.text = SaveManager.instance.levels[data.levelNum - 1].gems + "/4";
            // convert the float time into a timespan
            TimeSpan timeSpan = TimeSpan.FromSeconds(SaveManager.instance.levels[data.levelNum - 1].time);
            // set the text object to the timespan and format it
            time.text = timeSpan.ToString(format:@"mm\:ss\:ff");
            score.text = "Score: " + SaveManager.instance.levels[data.levelNum - 1].score;
            // might add this later -- not needed for now -- better if it was visual -- ****
            // stars.text = "Stars: " + SaveManager.instance.levels[data.levelNum - 1].stars;
        }
    }
}
