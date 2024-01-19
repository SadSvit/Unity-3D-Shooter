using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStepScript : MonoBehaviour
{
    private CharacterController characterController;

    private string currentAnimation = "";
    private string previousAnimation = "";

    [Header("Audio settings")]
    [SerializeField] private AudioClip[] footStepSoundsOnMetall;
    [SerializeField] private AudioClip[] footStepSoundsOnLadder;
    [SerializeField] private AudioSource playerAudio;

    [Header("Ray settings")]
    private RaycastHit groundHit;
    private float rayDistance;

    private void Start()
    {
        characterController = transform.root.GetComponent<CharacterController>();
        rayDistance = characterController.height / 2 + 0.35f;
    }

    public void PlayFootStepSound(string animationName)
    {
        currentAnimation = animationName;

        if (!playerAudio.isPlaying || currentAnimation == previousAnimation)
        {
            //индекс проигрываемого звука
            int randomIndex;

            //луч для проверки типа поверхности под персонажем
            Physics.Raycast(characterController.bounds.center, new Vector3(0, -rayDistance, 0), out groundHit, rayDistance, LayerMask.GetMask("Ground"));

            if (groundHit.collider.CompareTag("Ladder"))
            {
                randomIndex = Random.Range(0, footStepSoundsOnLadder.Length);
                playerAudio.PlayOneShot(footStepSoundsOnLadder[randomIndex]);
            }
            else
            {
                randomIndex = Random.Range(0, footStepSoundsOnMetall.Length);
                playerAudio.PlayOneShot(footStepSoundsOnMetall[randomIndex]);
            }
        }

        previousAnimation = currentAnimation;
    }
}
