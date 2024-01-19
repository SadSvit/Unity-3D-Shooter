using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerKillNotificationScript : MonoBehaviour
{
    [Header("UI settings")]
    [SerializeField] private Image PanelImage;
    [SerializeField] private Image WeaponImage;

    [SerializeField] private TMP_Text playerName_1;
    [SerializeField] private TMP_Text playerName_2;

    private float delay = 10.0f; // Длительность анимации затухания
    private float elapsedTime = 0f; //  время, прошедшее с начала анимации прозрачности

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(delay)); // Уничтожить объект через n секунды
    }

    private void Update()
    {
        float alphaValue = Mathf.Lerp(1f, 0f, elapsedTime / delay);

        PanelImage.color = new Color(PanelImage.color.r, PanelImage.color.g, PanelImage.color.b, alphaValue);
        WeaponImage.color = new Color(WeaponImage.color.r, WeaponImage.color.g, WeaponImage.color.b, alphaValue);

        playerName_1.color = new Color(playerName_1.color.r, playerName_1.color.g, playerName_1.color.b, alphaValue);
        playerName_2.color = new Color(playerName_2.color.r, playerName_2.color.g, playerName_2.color.b, alphaValue);

        elapsedTime += Time.deltaTime;
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
