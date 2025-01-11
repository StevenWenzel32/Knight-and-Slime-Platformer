using TMPro;
using UnityEngine;

public class DropdownLabelUpdater : MonoBehaviour
{
    public TMP_Dropdown dropdown;  // Reference to your dropdown
    public TextMeshProUGUI label;  // Reference to the label that displays the selected value

    private void Start()
    {   
        // Initialize the label with the current dropdown value
        UpdateLabel(dropdown.value);
    }

    // Method to update the label text
    public void UpdateLabel(int index)
    {
        label.text = dropdown.options[index].text;
    }
}

