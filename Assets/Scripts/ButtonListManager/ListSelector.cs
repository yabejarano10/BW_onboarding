using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ListSelector : MonoBehaviour
{
    public Sprite newButtonSprite;
    public Sprite oldButtonSprite;
    Button button;
    bool selected = false;
    public ListManager listManager;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeButtonImage);
    }
    public void ChangeButtonImage()
    {
        if (!selected)
        {
            if (listManager.getSelectedCount() < listManager.selectionLimit)
            {
                button.image.sprite = newButtonSprite;
                listManager.SelectItem();
                selected = !selected;
            }
        }
        else
        {
            button.image.sprite = oldButtonSprite;
            listManager.DeselectItem();
            
            selected = !selected;
        }
    }
}

