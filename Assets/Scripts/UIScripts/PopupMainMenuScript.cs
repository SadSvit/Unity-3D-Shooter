using UnityEngine;

public class PopupMainMenuScript : MenuScript
{
    [SerializeField] PopupManager popupMenuManager;

    private void Start()
    {
        MenuName = MenuNames.MainMenu;       
    }
    public void OpenSettingsMenu()
    {
        menuManager.OpenMenu(MenuNames.SettingsMenu);
    }

    public void Resume()
    {
        popupMenuManager.Disappear();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
