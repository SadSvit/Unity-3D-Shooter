using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PlayerSpawnScript : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] playerSpawnPoints;
    private Player[] players;
    private GameObject localPlayer;

    private void Start()
    {
        players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                localPlayer = PhotonNetwork.Instantiate(playerPrefab.name, playerSpawnPoints[i].position, Quaternion.identity);

                //Установка никнейма игрока
                localPlayer.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, PhotonNetwork.NickName);

                //определение игрока, как локального пользователя
                localPlayer.GetComponent<PlayerSetupScript>().IsLocalPlayer();                  
            }
        }
    }
  
}

