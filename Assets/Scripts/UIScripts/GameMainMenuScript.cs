using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMainMenuScript : MenuScript
{
    [SerializeField] PopupManager popupMenuManager;

    public static Action<Player> onPlayerLeftRoom;

    public void Start()
    {
        MenuName = MenuNames.MainMenu;
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        onPlayerLeftRoom(otherPlayer);
    }

    public void Exit()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("LoadingScene");
    }
    public void OpenSettingsMenu()
    {
        menuManager.OpenMenu(MenuNames.SettingsMenu);
    }

    public void Resume()
    {
        popupMenuManager.Disappear();
    }   
}
