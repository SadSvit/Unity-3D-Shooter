using Photon.Pun;
using TMPro;
using UnityEngine;

public class EnterNameMenuScript : MenuScript
{
    [SerializeField] private TMP_InputField playerNameInputField;
    
    public void Start()
    {        
        MenuName = MenuNames.EnterNameMenu;
    }

    public void SetPlayerName()
    {
        PhotonNetwork.NickName = playerNameInputField.text;
        menuManager.OpenMenu(MenuNames.MainMenu);
    }
}
