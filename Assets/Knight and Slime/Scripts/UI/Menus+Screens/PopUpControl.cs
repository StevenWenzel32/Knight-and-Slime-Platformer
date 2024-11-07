using UnityEngine;

public class PopUpControl : MonoBehaviour
{
    // give it the pop up to control
    public GameObject popUpPanel;

    public void ShowPopUp(){
        popUpPanel.SetActive(true);
    }

    public void HidePopUp(){
        popUpPanel.SetActive(false);
    }
}
