using System;
using UnityEngine;

public class GamePopupMenuManagerScript : PopupManager
{
    [Header("Action settings")]
    public static Action onPopupMenuAppear;
    public static Action onPopupMenuDisappear;

    public override void Start()
    {
        isOpen = false;
        popupControllerButton = KeyCode.Escape;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Appear()
    {
        base.Appear();
        onPopupMenuAppear.Invoke();
    }

    public override void Disappear()
    {
        base.Disappear();
        onPopupMenuDisappear.Invoke();
    }
}
