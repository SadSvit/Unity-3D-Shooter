using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerListItemScript : MonoBehaviourPunCallbacks
{
    private Player player;
    [SerializeField] private TMP_Text playerNameText;

    public void PlayerSetUp(Player player)
    {
        this.player = player;
        playerNameText.text = player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        if (this.player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        Destroy(gameObject);
    }
}
