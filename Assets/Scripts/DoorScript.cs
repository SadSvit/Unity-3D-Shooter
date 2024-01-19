using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [Header("Audio settings")]
    [SerializeField] private AudioClip openDoorSound;
    [SerializeField] private AudioClip closeDoorSound;
    [SerializeField] private AudioSource doorAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Animator>().SetTrigger("Open");
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Animator>().SetTrigger("Close");
        }
    }
    public void PlayDoorOpenSound()
    {
        doorAudio.PlayOneShot(openDoorSound);
    }
    public void PlayDoorCloseSound()
    {
        doorAudio.PlayOneShot(closeDoorSound);
    }
}
