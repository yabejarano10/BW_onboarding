using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SelectAvatar : MonoBehaviour
{

    Button button;
    public Image mainAvatar;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeAvatar);
    }

    // Update is called once per frame
    public void ChangeAvatar()
    {
        Color newColor = button.gameObject.GetComponent<Image>().color;
        mainAvatar.color = newColor;
    }
}
