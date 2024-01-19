using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Networking;
public class PlayerBodyAnimationScript : MonoBehaviourPunCallbacks, IPunObservable
{

    [Header("General settings")]
    [SerializeField] private Animator playerBodyAnimator;

    private float verticalInput = 0;
    private float horizontalInput = 0;

    [Header("Rig settings")]
    [SerializeField] private Rig playerBodyRig;

    [Header("Synchronization settings")]
    private PhotonView photonView;

    [SerializeField] private float syncStartVerticalInput = 0; //начальное число вертикального ввода интерполяции 
    [SerializeField] private float syncStartHorizontalInput = 0; //начальное число горизонтального ввода интерполяции 

    [SerializeField] private float syncEndVerticalInput = 0; // конечное число вертикального ввода  интерполяции
    [SerializeField] private float syncEndHorizontalInput = 0; // конечное число горизонтального ввода интерполяции

    [SerializeField] private float lastSynchronizationTime; // последнее время синхронизации
    [SerializeField] private float syncDelay = 0f; // дельта между текущим временем и последней синхронизацией
    [SerializeField] private float syncTime = 0f; // время синхронизации

    void Start()
    {
        lastSynchronizationTime = Time.time;
        photonView = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            // Получение ввода для активации анимаций
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");

            if (Input.GetKey(KeyCode.LeftShift))
            {
                verticalInput *= 2f;
                horizontalInput *= 2f;
            }

            //применяем ввод локального игрока в blend tree    
            playerBodyAnimator.SetFloat("Vertical", verticalInput);
            playerBodyAnimator.SetFloat("Horizontal", horizontalInput);
        }
        else
        {
            syncTime += Time.deltaTime;

            verticalInput = Mathf.Lerp(syncStartVerticalInput, syncEndVerticalInput, syncTime / syncDelay); // интерполяция вертикального ввода
            horizontalInput = Mathf.Lerp(syncStartHorizontalInput, syncEndHorizontalInput, syncTime / syncDelay);// интерполяция горизонтального ввода

            //применяем ввод других игроков в blend tree  
            playerBodyAnimator.SetFloat("Vertical", verticalInput); 
            playerBodyAnimator.SetFloat("Horizontal", horizontalInput);

        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        float syncVerticalInput; // для синхронизации вертикального ввода
        float syncHorizontalInput; // для синхронизации горищонтального ввода

        if (stream.IsWriting) // если отправляем в сеть, то считываем данные объекта перед отправкой
        {
            syncVerticalInput = verticalInput;
            stream.SendNext(syncVerticalInput);

            syncHorizontalInput = horizontalInput;
            stream.SendNext(syncHorizontalInput);
        }
        else
        {
            syncVerticalInput = (float)stream.ReceiveNext();
            syncHorizontalInput = (float)stream.ReceiveNext();

            syncTime = 0f; // сбрасываем время синхронизации
            syncDelay = Time.time - lastSynchronizationTime; // получаем дельту предыдущей синхронизации
            lastSynchronizationTime = Time.time; // записываем новое время последней синхронизации

            if (syncVerticalInput != 0)
            {
                syncEndVerticalInput = syncVerticalInput + syncDelay * 0.5f; // конечная  величина вертикального ввода, к которой будет стримиться анимация в blend tree
                syncStartVerticalInput = verticalInput; // начальная величина по вертикали в blend tree
            }
            else
            {
                syncEndVerticalInput = 0;
                syncStartVerticalInput = 0;
            }

            if (syncHorizontalInput != 0)
            {
                syncEndHorizontalInput = syncHorizontalInput + syncDelay * 0.5f; // конечная  величина горизонтального ввода, к которой будет стримиться анимация в blend tree
                syncStartHorizontalInput = horizontalInput; 
            }
            else
            {
                syncEndHorizontalInput = 0;
                syncStartHorizontalInput = 0;
            }
        }
    }

    public void StartDeathAnimation(string bodyPart, Vector3 hitNormal)
    {
        playerBodyRig.weight = 0;
        playerBodyAnimator.applyRootMotion = true;

        playerBodyAnimator.SetFloat("HitNormalX", hitNormal.x);
        playerBodyAnimator.SetFloat("HitNormalZ", hitNormal.z);
                
        if (bodyPart == "PlayerHead")
        {
            playerBodyAnimator.SetTrigger("DeathHead");
        }
        else
        {
            playerBodyAnimator.SetTrigger("Death");
        }
    }

    [PunRPC]
    public void StartJumpAnimation()
    {
        playerBodyAnimator.SetTrigger("Jump");
    }
    [PunRPC]
    public void StartReloadAnimation()
    {
        playerBodyAnimator.SetTrigger("Reload");
    }
   
    [PunRPC]
    public void StartLandAnimation()
    {
        playerBodyAnimator.SetTrigger("Land");
    }

    public bool IsAnimationPlaying(string name)
    {

        // Получаем информацию о текущем состоянии аниматора
        AnimatorStateInfo stateInfo = playerBodyAnimator.GetCurrentAnimatorStateInfo(0);

        // Сравниваем имя текущей анимации с искомым именем
        if (stateInfo.IsName(name))
        {
            return true;
        }

        return false; // Анимация не проигрывается сейчас
    }
}
