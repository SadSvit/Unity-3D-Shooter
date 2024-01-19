using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCavasScript : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private float refreshRate = 1f; // Частота обновления лидерборда

    [Header("UI")]
    public GameObject leaderBoard;

    public GameObject gameEndPanel;
    public GameObject gameResultImage;

    public TMP_Text gameResultText;
    public TMP_Text countDownText;

    private void OnEnable()
    {
        PlayerHealthScript.onLocalPlayerDeath += ShowLeaderBoard;
        GameManagerScript.onGameEnd += ShowFinalBoard;
    }

    private void OnDisable()
    {
        PlayerHealthScript.onLocalPlayerDeath -= ShowLeaderBoard;
        GameManagerScript.onGameEnd -= ShowFinalBoard;
    }

    public void ShowLeaderBoard()
    {
        leaderBoard.SetActive(true);
    }

    public void ShowGameEndPanel()
    {
        gameEndPanel.SetActive(true);
    }

    public void ShowFinalBoard()
    {
        //оставшийся в живых игрок = победил
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!player.GetComponent<PlayerHealthScript>().isDeath)
            {
                gameResultText.text = player.GetComponent<PhotonView>().Owner.NickName + " Win !";
            }
        }

        leaderBoard.SetActive(true);
        gameEndPanel.SetActive(true);
    }
}
