using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CanvasType
{
    MainCanvas,
    WelcomeCanvas,
    ParentInterestCanvas,
    CustomizeCanvas,
    BirthdayCanvas,
    MonthCanvas,
    LanguageCanvas,
    FavoriteCanvas,
    FavoriteThingsCanvas,
    AvatarCanvas,
    SignUpCanvas,
    LoginCanvas,
    Modal
}
public class CanvasManager : Singleton<CanvasManager>
{
    List<CanvasController> canvasControllerList;
    CanvasController lastActiveCanvas;
    FirebaseAuthManager fb;

    protected override void Awake()
    {
        fb = FindObjectOfType<FirebaseAuthManager>();
        canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();

        canvasControllerList.ForEach(x => x.gameObject.SetActive(false));

        switchCanvas(CanvasType.MainCanvas);
    }

    public void switchCanvas(CanvasType type)
    {
        if(lastActiveCanvas != null && type != CanvasType.Modal)
        {
            lastActiveCanvas.gameObject.SetActive(false);
        }

        CanvasController newCanvas = canvasControllerList.Find(x => x.canvasType == type);
        if (newCanvas != null)
        {
            newCanvas.gameObject.SetActive(true);
            lastActiveCanvas = newCanvas;
        }
    }

    public void LogIn()
    {
        fb.Login();
    }
    public void Register()
    {
        fb.Register();
    }

    public void GoogleLogIn()
    {
        fb.GoogleLogIn();
    }

}
