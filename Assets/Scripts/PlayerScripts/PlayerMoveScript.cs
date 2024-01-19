using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PlayerMoveScript : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("General settings")]
    private CharacterController characterController;

    [Header("Action settings")]
    public Action onIdle;
    public Action onWalk;
    public Action onRun;

    [Header("Walk settings")]
    private Vector3 movement;
    [SerializeField] private float playerSpeed;

    float horizontalInput;
    float verticalInput;

    [Header("jump settings")]
    [SerializeField] private float jumpSpeed;

    [Header("Gravity settings")]
    private float gravity;
    private float verticalSpeed;

    private float minFall;
    private bool isGrounded;
    private RaycastHit groundHit;

    [Header("Multiplayer settings")]
    private Vector3 networkMoveDirection; // ��������, ���������� �� ������ ��������

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        playerSpeed = 2f;
        jumpSpeed = 9.0f;
        gravity = -9.8f;

        minFall = -1.5f;
        verticalSpeed = minFall;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            movement = Vector3.zero;

            // ��������� ����� ��� ������������
            ApplyGravityToPlayer();
            StartCoroutine(PlayerMakeJump());

            //����� �� ����� ������ ����������� �������� �� ����� �������
            if (isGrounded)
            {
                horizontalInput = Input.GetAxis("Horizontal");
                verticalInput = Input.GetAxis("Vertical");

                //��������� �����
                if (horizontalInput == 0 && verticalInput == 0)
                {
                    onIdle.Invoke();
                }
                //���
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    horizontalInput *= 2f;
                    verticalInput *= 2f;
                    onRun.Invoke();
                }
                //������
                else
                {
                    onWalk.Invoke();
                }
            }

            movement.x = horizontalInput;
            movement.z = verticalInput;
            movement.y = verticalSpeed;

            movement = transform.TransformDirection(movement) * playerSpeed;

            characterController.Move(movement * Time.deltaTime);

            // �������� �������� ����� RPC
            photonView.RPC("UpdateMoveDirection", RpcTarget.AllBuffered, movement);
        }
        else
        {
            // ���������� ��������, ����������� �� ������ ��������
            characterController.Move(networkMoveDirection * Time.deltaTime);
        }

    }

    // RPC-����� ��� �������� �������� ����� ���������
    [PunRPC]
    private void UpdateMoveDirection(Vector3 movement)
    {
        networkMoveDirection = movement;
    }

    // ������ ���������� IPunObservable ��� ������� �������������
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(movement);
        }
        else
        {
            networkMoveDirection = (Vector3)stream.ReceiveNext();
        }
    }
    private void ApplyGravityToPlayer()
    {
        float rayDistance = characterController.height / 2 + 0.2f;

        Physics.Raycast(characterController.bounds.center, new Vector3(0, -rayDistance, 0), out groundHit, rayDistance, LayerMask.GetMask("Ground"));

        if (groundHit.collider != null)
        {
            //���� ������������� �������� ������ ��� ��������� � �������, �� ��� ���������� ������ ��������� �������� ����������� 
            if (GetComponent<PlayerBodyAnimationScript>().IsAnimationPlaying("jump loop") || GetComponent<PlayerBodyAnimationScript>().IsAnimationPlaying("jump up"))
            {
                GetComponent<PhotonView>().RPC("StartLandAnimation", RpcTarget.AllBuffered);
            }

            isGrounded = true;

        }
        else
        {
            isGrounded = false;
        }
        if (!isGrounded)
        {
            verticalSpeed += gravity * 3 * Time.deltaTime;
        }
    }
    private IEnumerator PlayerMakeJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                yield return new WaitForSeconds(0.08f);
                verticalSpeed = jumpSpeed;
                GetComponent<PhotonView>().RPC("StartJumpAnimation", RpcTarget.AllBuffered);
            }
            else
            {
                verticalSpeed = minFall;
            }
        }
    }
}