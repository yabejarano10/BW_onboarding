using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListManager : MonoBehaviour
{
    int selectedButtons = 0;
    public int selectionLimit = 0;
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
        return (selectedButtons > 0 && selectedButtons <= selectionLimit);
    }

    public int getSelectedCount()
    {
        return selectedButtons;
    }
}
