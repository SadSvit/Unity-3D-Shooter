using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopupNotificationScript : MonoBehaviour
{
    private float countdownTimer = 3;

    [Header("UI settings")]
    [SerializeField] private GameObject antiClicker;
    [SerializeField] private GameObject closeNotifiationButton;
    [SerializeField] private TMP_Text countDownText;
    [SerializeField] private TMP_Text notificationText;

    [Header("Audio settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip notificationSound;

    private void OnEnable()
    {
        MainMenuScript.onErorAppear += AppearNotification;
    }

    private void OnDisable()
    {
        MainMenuScript.onErorAppear -= AppearNotification;
    }

    public void AppearNotification(string notificationText)
    {
        this.notificationText.text = notificationText;
        antiClicker.SetActive(true);
        GetComponent<Animator>().SetTrigger("Appear");
        audioSource.PlayOneShot(notificationSound);
    }

    public void CloseNotification()
    {
        GetComponent<Animator>().SetTrigger("Disappear");
        audioSource.PlayOneShot(notificationSound);
    }

    public IEnumerator StartCountdown()
    {
        float tempTimer = countdownTimer;
        countDownText.text = countdownTimer.ToString();

        while (tempTimer >= 0f)
        {
            yield return new WaitForSeconds(1f); // Ждем 1 секунду
            tempTimer--;
            countDownText.text = tempTimer.ToString();

            // Если таймер истек, выходим из комнаты
            if (tempTimer <= 0f)
            {
                GetComponent<Animator>().SetTrigger("AppearIdle");
                break;
            }
        }

    }
}
