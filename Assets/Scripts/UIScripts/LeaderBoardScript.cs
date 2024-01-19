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
    [SerializeField] private float refreshRate = 1f; // ������� ���������� ����������

    [Header("UI")]
    public GameObject[] slots; // ������ ������ ��� ����������� �������
    [Space]
    public TextMeshProUGUI[] scoreTexts; // ������ ��������� ��������� ��� ����������� ����� �������
    public TextMeshProUGUI[] nameTexts; // ������ ��������� ��������� ��� ����������� ���� �������

    void Start()
    {
        // ��������� ������������� ���������� ����������
        InvokeRepeating(nameof(Refresh), refreshRate, refreshRate);
    }   

    public void Refresh()
    {
        foreach (GameObject slot in slots)
        {
            slot.SetActive(false); // �������� ��� �����
        }

        // �������� ��������������� ������ ������� �� ����� (�� ���������� � �����������)
        List<Player> sortedPlayersList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

        int i = 0;

        // ��������� ����������� ��� ������� ������ � ����������
        foreach (Player player in sortedPlayersList)
        {
            slots[i].SetActive(true); // ���������� ����

            nameTexts[i].text = player.NickName; // ���������� ��� ������
            scoreTexts[i].text = player.GetScore().ToString(); // ���������� ���� ������

            i++;
        }
    }
}
