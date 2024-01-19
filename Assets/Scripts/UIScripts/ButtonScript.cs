using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [Header("Audio settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    [Header("Sprite settings")]
    public Sprite enableButtonSprite;
    public Sprite disableButtonSprite;

    Image buttonImage;

    public void PointerEnter()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = enableButtonSprite;

        audioSource.PlayOneShot(hoverSound);
    }

    public void PointerExit()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = disableButtonSprite;

    }

    public void PointerClick()
    {
        audioSource.PlayOneShot(clickSound);
    }    
}

