using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuScript : MenuScript
{
    [Header("General settings")]
    private List<string> namesOfAvailableRooms = new List<string>();
    private List<RoomInfo> availableRooms = new List<RoomInfo>();

    private float timeBetweenUpdates = 1.5f;
    private float nextUpdate;

    [Header("UI settings")]
    [SerializeField] private TMP_InputField createRoomInputField;
    [SerializeField] private TMP_InputField connetToRoomInputField;

    [Header("Action settings")]
    public static Action<string> onErorAppear;

    public void Start()
    {        
        MenuName = MenuNames.MainMenu;
    }

    //Создание комнаты
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createRoomInputField.text))
        {
            return;
        }

        if (!namesOfAvailableRooms.Contains(createRoomInputField.text))
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.CustomRoomPropertiesForLobby = new string[] { LobbyManager.ROOM_CURRENT_SCENE };
            roomOptions.CustomRoomProperties = new Hashtable { { LobbyManager.ROOM_CURRENT_SCENE, "Lobby" } };
            roomOptions.MaxPlayers = 4;

            PhotonNetwork.CreateRoom(createRoomInputField.text, roomOptions, null);
            menuManager.OpenMenu(MenuNames.LoadingMenu);
        }
        else
        {
            onErorAppear("Room with the same name already exists");
        }
    }

    //Вход в комнату
    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(connetToRoomInputField.text))
        {
            return;
        }
        //проверка существует ли комната с таким названием
        if (namesOfAvailableRooms.Contains(connetToRoomInputField.text))
        {
            foreach (RoomInfo room in availableRooms)
            {
                if (room.Name == connetToRoomInputField.text)
                {
                    if (room.CustomProperties.TryGetValue("CurrentScene", out var currentScene))
                    {
                        if (currentScene.Equals("Lobby"))
                        {
                            PhotonNetwork.JoinRoom(connetToRoomInputField.text);
                            menuManager.OpenMenu(MenuNames.LoadingMenu);
                        }
                        else
                        {
                            Debug.Log(room.CustomProperties);
                            onErorAppear("The game in this room is already running");
                        }
                    }
                    break; // Выход из цикла, так как комната найдена
                }
            }
        }
        else
        {
            onErorAppear("There is no room with this name");
        }
    }

    //Ошибка при создании комнаты
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        onErorAppear("Error:" + message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        if (Time.time > nextUpdate)
        {
            // Очищаем предыдущий список доступных комнат
            availableRooms.Clear();
            // Проходимся по списку комнат и сохраняем их имена
            foreach (RoomInfo room in roomList)
            {
                availableRooms.Add(room);
                namesOfAvailableRooms.Add(room.Name);
            }

            nextUpdate = Time.time + timeBetweenUpdates;
        }
    }
}
