using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using System;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public static readonly string ROOM_CURRENT_SCENE = "CurrentScene";
}

