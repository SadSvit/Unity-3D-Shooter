 using Photon.Pun;
 using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun.UtilityScripts;

public class LeaderBoardScript : MonoBehaviour
{  
    [Header("Options")]
    [SerializeField] private float refreshRate = 1f; // Частота обновления лидерборда

    [Header("UI")]
    public GameObject[] slots; // Массив слотов для отображения игроков
    [Space]
    public TextMeshProUGUI[] scoreTexts; // Массив текстовых элементов для отображения счета игроков
    public TextMeshProUGUI[] nameTexts; // Массив текстовых элементов для отображения имен игроков

    void Start()
    {
        // Запускаем периодическое обновление лидерборда
        InvokeRepeating(nameof(Refresh), refreshRate, refreshRate);
    }   

    public void Refresh()
    {
        foreach (GameObject slot in slots)
        {
            slot.SetActive(false); // Скрываем все слоты
        }

        // Получаем отсортированный список игроков по счету (от наивысшего к наименьшему)
        List<Player> sortedPlayersList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

        int i = 0;

        // Обновляем отображение для каждого игрока в лидерборде
        foreach (Player player in sortedPlayersList)
        {
            slots[i].SetActive(true); // Показываем слот

            nameTexts[i].text = player.NickName; // Отображаем имя игрока
            scoreTexts[i].text = player.GetScore().ToString(); // Отображаем счет игрока

            i++;
        }
    }
}
