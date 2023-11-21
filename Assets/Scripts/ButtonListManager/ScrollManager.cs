using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : Singleton<ScrollManager>
{
    int selectedButtons = 0;
    Button currentSelectedButton;
    public void SelectItem()
    {
        selectedButtons++;
    }

    // Update is called once per frame
    public void DeselectItem()
    {
        selectedButtons--;
    }

    public bool canContinue()
    {
        return (selectedButtons > 0 && selectedButtons <= 3);
    }

    public int getSelectedCount()
    {
        return selectedButtons;
    }

    public void setSelectedButton(Button current)
    {
        currentSelectedButton = current;
    }
    public void clearSelection()
    {
        if(currentSelectedButton != null)
        {
            currentSelectedButton.GetComponent<ScrollSelector>().deselectButton();
        }
    }
}
