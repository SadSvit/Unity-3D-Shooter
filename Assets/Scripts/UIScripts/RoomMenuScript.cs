using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomMenuScript : MenuScript
{
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private Transform playersList;
    [SerializeField] private GameObject playerNamePrefab;
    [SerializeField] private GameObject startGameButton; 
    [SerializeField] private IMultiPlayerMenuManager multiPlayerMenuManager;
    [SerializeField] public  GameObject Manager { get; set; }

    public void Start()
    {
        MenuName = MenuNames.RoomMenu;
        multiPlayerMenuManager = (IMultiPlayerMenuManager) menuManager;
    }

    //������ �����
    public void StartGame()
    {
        multiPlayerMenuManager.OpenMenuForAllPlayers(MenuNames.LoadingMenu);
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { LobbyManager.ROOM_CURRENT_SCENE, "Game" } });
        PhotonNetwork.LoadLevel("Game");
    }

    //����� �� �������
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

        //������� ������� �� ����� �������
        for (int i = 0; i < playersList.childCount; i++)
        {
            Destroy(playersList.GetChild(i).gameObject);
        }

        //���������� ������� � ����  �������
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerNamePrefab, playersList).GetComponent<PlayerListItemScript>().PlayerSetUp(players[i]);
        }

        //���������� ������ ������ ��� �������������� �������
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
 