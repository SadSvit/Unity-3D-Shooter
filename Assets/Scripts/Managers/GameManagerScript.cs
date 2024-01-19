using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManagerScript : MonoBehaviourPunCallbacks
{
    private int livePlayersCounter;

    [Header("Game end settings")]

    [SerializeField] private TMP_Text countdownText;
    private float countdownTimer = 20f; // ����� � ��������

    [Header("Coins settings")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private List<GameObject> coins;
    [SerializeField] private List<Transform> coinSpawnPoints;

    public static Action onGameEnd;

    public override void OnEnable()
    {
        PlayerHealthScript.OnSpawn += LivePlayersCounterUpdate;
        PlayerHealthScript.onAnotherPlayerDeath += GameEndCheck;
        PlayerHealthScript.onLocalPlayerDeath += GameEndCheck;
    }

    public override void OnDisable()
    {
        PlayerHealthScript.OnSpawn -= LivePlayersCounterUpdate;
        PlayerHealthScript.onAnotherPlayerDeath -= GameEndCheck;
        PlayerHealthScript.onLocalPlayerDeath -= GameEndCheck;
    }


    public void Start()
    {
        InvokeRepeating(nameof(SpawnCoin), 10, 10);
    }  

    //����� ��� ������ �����
    private void SpawnCoin()
    {
        List<int> idsOfEmptySpawnPoints = new List<int>();

        // ���������� ��� ����� ������ ����� � ��������� ������ � ����
        for (int i = 0; i < coins.Count; i++)
        {
            if (coins[i] == null)
            {
                idsOfEmptySpawnPoints.Add(i);
            }
        }

        //���� ���� ����� ������ ��� �����, �� ��������� ������� �������� ��� ���������� ������
        if (idsOfEmptySpawnPoints.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, idsOfEmptySpawnPoints.Count - 1);
            int randomId = idsOfEmptySpawnPoints[randomIndex];
            coins[randomId] = Instantiate(coinPrefab, new Vector3(0, 0, 0), Quaternion.identity, coinSpawnPoints[randomId]);
        }
    }

    //����� ������� �������� ������, ����������� ������� ��������� ����
    private void GameEndCheck()
    {
        livePlayersCounter = 0;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!player.GetComponent<PlayerHealthScript>().isDeath)
            {
                livePlayersCounter++;
            }
        }

        //���� ������� ���� �����, �� ����� �������������
        if (livePlayersCounter <= 1)
        {
            GameEnd();
        }
    }

    private void LivePlayersCounterUpdate()
    {
        livePlayersCounter++;
    }

    private void GameEnd()
    {
        onGameEnd.Invoke();
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        while (countdownTimer > 0f)
        {
            yield return new WaitForSeconds(1f); // ���� 1 �������
            countdownTimer--;

            // ��������� ����� ��������� �������
            if (countdownText != null)
            {
                countdownText.text = countdownTimer.ToString() + " seconds to exit";
            }

            // ���� ������ �����, ������� �� �������
            if (countdownTimer <= 0f)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel("Lobby");
            }
        }
    }
}