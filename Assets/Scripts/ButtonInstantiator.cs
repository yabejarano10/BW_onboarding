using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ScrollTypes
{
    years,
    months,
    languages
}
public class ButtonInstantiator : MonoBehaviour
{
    public GameObject buttonPrefab;
    Transform buttonParent;
    public ScrollTypes type;
    void Start()
    {
        buttonParent = this.transform;
        if (buttonPrefab != null && buttonParent != null)
        {
            if(type == ScrollTypes.years)
            {
                List<int> listYears = Enumerable.Range(2005, DateTime.Now.Year - 2005 + 1).ToList();
                listYears.ForEach(year =>
                {
                    GameObject newButton = Instantiate(buttonPrefab, buttonParent);
                    TextMeshProUGUI buttonTextComponent = newButton.GetComponentInChildren<TextMeshProUGUI>();
                    buttonTextComponent.text = year.ToString();
                });

            }
            else if(type == ScrollTypes.months)
            {
                DateTimeFormatInfo dateFormatInfo = CultureInfo.GetCultureInfo("en-GB").DateTimeFormat;
                string[] names = dateFormatInfo.MonthNames;
                for(int i = 0; i< names.Length;i++)
                {
                    GameObject newButton = Instantiate(buttonPrefab, buttonParent);
                    TextMeshProUGUI buttonTextComponent = newButton.GetComponentInChildren<TextMeshProUGUI>();
                    buttonTextComponent.text = names[i];
                }
            }
            else
            {
                List<string> languages = new List<string> { "Spanish", "English", "Portuguese", "Italian", "French", "Russian" };
                languages.ForEach(language =>
                {
                    GameObject newButton = Instantiate(buttonPrefab, buttonParent);
                    TextMeshProUGUI buttonTextComponent = newButton.GetComponentInChildren<TextMeshProUGUI>();
                    buttonTextComponent.text = language;
                });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
