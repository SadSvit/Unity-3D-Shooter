using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerHealthScript>())
        {
            if (other.GetComponent<PhotonView>().Owner.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                PhotonNetwork.LocalPlayer.AddScore(5);
            }
            
            Destroy(gameObject);
        }
    }
}
