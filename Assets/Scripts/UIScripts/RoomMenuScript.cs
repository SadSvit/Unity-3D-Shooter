using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomMenuScript : MenuScript
{
    [Header("General settings")]
    [SerializeField] private Transform playersList;
    [SerializeField] private GameObject playerNamePrefab;
    [SerializeField] private IMultiPlayerMenuManager multiPlayerMenuManager;
    
    [Header("UI settings")]
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private GameObject startGameButton;

    public void Start()
    {
        MenuName = MenuNames.RoomMenu;
        multiPlayerMenuManager = (IMultiPlayerMenuManager) menuManager;
    }

    //Запуск игрыы
    public void StartGame()
    {
        multiPlayerMenuManager.OpenMenuForAllPlayers(MenuNames.LoadingMenu);
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { LobbyManager.ROOM_CURRENT_SCENE, "Game" } });
        PhotonNetwork.LoadLevel("Game");
    }

    //Выход из комнаты
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        menuManager.OpenMenu(MenuNames.LoadingMenu);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        //Очистка игроков из листа игроков
        for (int i = 0; i < playersList.childCount; i++)
        {
            Destroy(playersList.GetChild(i).gameObject);
        }

        //Добавление игроков в лист  игроков
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerNamePrefab, playersList).GetComponent<PlayerListItemScript>().PlayerSetUp(players[i]);
        }

        //отключение кнопки старта для подключившихся игроков
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

        menuManager.OpenMenu(MenuNames.RoomMenu);
    }

    public override void OnLeftRoom()
    {
        menuManager.OpenMenu(MenuNames.MainMenu);
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        base.OnPlayerEnteredRoom(player);

        Instantiate(playerNamePrefab, playersList).GetComponent<PlayerListItemScript>().PlayerSetUp(player);
    }
}
 