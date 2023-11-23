using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomePage : MonoBehaviour
{
    FirebaseAuthManager fb;
    void Awake()
    {
        fb = FindObjectOfType<FirebaseAuthManager>();
    }

    public void LogOut()
    {
        fb.LogOut();
    }
}
