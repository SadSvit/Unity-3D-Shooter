using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerCanvasScript : MonoBehaviour
{
    [Header("UI settings")]
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private GameObject playerKillNotificationPanel;
    [SerializeField] private GameObject playerKillsPanel;

    public void ChangePlayerHealthText(int playerHealth)
    {
        playerHealthText.text = playerHealth.ToString();
    }

    [PunRPC]
    public void PushKillNotification(string playerName_1, string playerName_2)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject localPlayer = null;

        foreach (GameObject player in players)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                localPlayer = player;
            }
        }

        playerKillNotificationPanel.transform.Find("PlayerName_1").GetComponent<TMP_Text>().text = playerName_1;
        playerKillNotificationPanel.transform.Find("PlayerName_2").GetComponent<TMP_Text>().text = playerName_2;

        Instantiate(playerKillNotificationPanel, localPlayer.transform.Find("PlayerCanvas").transform.Find("PlayerKillsPanel").transform);
    }
}

