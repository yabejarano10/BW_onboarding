using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WelcomePage : MonoBehaviour
{
    FirebaseAuthManager fb;
    public TMP_Text name;
    void Awake()
    {
        fb = FindObjectOfType<FirebaseAuthManager>();
    }
    private void Start()
    {
        name.text = fb.user.DisplayName; 
    }

    public void LogOut()
    {
        fb.LogOut();
    }
}
