using UnityEngine;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static readonly string ROOM_CURRENT_SCENE = "CurrentScene";

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }    
}

