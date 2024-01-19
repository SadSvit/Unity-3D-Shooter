using Photon.Pun;

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
