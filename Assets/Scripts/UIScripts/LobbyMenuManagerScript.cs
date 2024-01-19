using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenuManagerScript : MenuManagerScript, IMultiPlayerMenuManager
{
    public void OpenMenuForAllPlayers(MenuNames menuToOpen)
    {
        photonView.RPC("OpenMenu", RpcTarget.All, menuToOpen);
    }
    
    [PunRPC]
    public override void OpenMenu(MenuNames menuToOpen)
    {
        base.OpenMenu(menuToOpen);
    }
}
