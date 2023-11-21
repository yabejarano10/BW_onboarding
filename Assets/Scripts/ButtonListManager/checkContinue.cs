using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class checkContinue : MonoBehaviour
{

    Button button;
    public ListManager listManager;
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (listManager != null && !listManager.canContinue())
        {
            button.enabled = false;
        }
        else
        {
            button.enabled = true;
        }
    }
}
