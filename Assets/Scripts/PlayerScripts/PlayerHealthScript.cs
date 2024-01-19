using Photon.Pun;
using System;
using UnityEngine;
using Photon.Realtime;

public class PlayerHealthScript : MonoBehaviourPunCallbacks
{
    [Header("General settings")]
    [SerializeField] public int playerHealth;
    [SerializeField] public bool isDeath = false;
    [SerializeField] private Camera playerCamera;

    [Header("Action settings")]
    public static Action OnSpawn;
    public static Action onAnotherPlayerDeath;
    public static Action onLocalPlayerDeath;

    public override void OnEnable()
    {
        base.OnEnable();
        GameMainMenuScript.onPlayerLeftRoom += PlayerDeathAfterLeaveRoom;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        GameMainMenuScript.onPlayerLeftRoom -= PlayerDeathAfterLeaveRoom;
    }

    void Start()
    {
        playerHealth = 100;
        OnSpawn();
    }

    [PunRPC]
    public void GetHit(int damage, string bodyPartName, Vector3 hitNormal)
    {
        playerHealth -= damage;
        GetComponent<PlayerCanvasScript>().ChangePlayerHealthText(playerHealth);

        if (playerHealth > 0)
        {
            //get hit animation
        }
        else
        {
            isDeath = true;
              
            GetComponent<PlayerBodyAnimationScript>().StartDeathAnimation(bodyPartName, hitNormal);
            GetComponent<PlayerSetupScript>().PlayerDeathSetup();           

            if (photonView.IsMine)
            {
                Debug.Log("onLocalPlayerDeath");
                onLocalPlayerDeath?.Invoke();
            }
            else
            {
                Debug.Log("onAnotherPlayerDeath");
                onAnotherPlayerDeath?.Invoke();
            }
        }
    }

    public void PlayerDeathAfterLeaveRoom(Player otherPlayer)
    {
        if (otherPlayer.NickName == GetComponent<PhotonView>().Owner.NickName)
        {      
            GetComponent<PlayerSetupScript>().PlayerDeathSetup();
            onAnotherPlayerDeath?.Invoke();
        }
    }

}
