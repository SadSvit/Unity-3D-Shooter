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
    private float countdownTimer = 20f; // Время в секундах

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

    //метод для спавна монет
    private void SpawnCoin()
    {
        List<int> idsOfEmptySpawnPoints = new List<int>();

        // перебираем все точки спавна монет и добавляем пустые в лист
        for (int i = 0; i < coins.Count; i++)
        {
            if (coins[i] == null)
            {
                idsOfEmptySpawnPoints.Add(i);
            }
        }

        //если есть точки спавна без монет, то случайным образом выбираем где заспавнить монету
        if (idsOfEmptySpawnPoints.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, idsOfEmptySpawnPoints.Count - 1);
            int randomId = idsOfEmptySpawnPoints[randomIndex];
            coins[randomId] = Instantiate(coinPrefab, new Vector3(0, 0, 0), Quaternion.identity, coinSpawnPoints[randomId]);
        }
    }

    //после каждого убийства игрока, проверяется условие окончание игры
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

        //если остался один игрок, то играу заканчивается
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
            yield return new WaitForSeconds(1f); // Ждем 1 секунду
            countdownTimer--;

            // Обновляем текст обратного отсчета
            if (countdownText != null)
            {
                countdownText.text = countdownTimer.ToString() + " seconds to exit";
            }

            // Если таймер истек, выходим из комнаты
            if (countdownTimer <= 0f)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel("Lobby");
            }
        }
    }
}