using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ScrollSelector : MonoBehaviour
{
    public Sprite newButtonSprite;
    public Sprite oldButtonSprite;
    Button button;
    bool selected = false;
    ScrollManager Manager;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeButtonImage);
        Manager = ScrollManager.GetInstance();
    }
    public void ChangeButtonImage()
    {
        if (!selected)
        {
            button.image.sprite = newButtonSprite;
            selected = !selected;
            Manager.clearSelection();
            Manager.setSelectedButton(button);
        }
        else
        {
            deselectButton();
        }
    }

    public void deselectButton()
    {
        button.image.sprite = oldButtonSprite;
        selected = !selected;
    }
}

