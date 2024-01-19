using Photon.Pun;
using UnityEngine;

public class PlayerMouseLookScript : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("General settings")]
    [SerializeField] private Camera playerCamera;      // ������ ������
    [SerializeField] private GameObject playerHands;      //���� ������  

    [SerializeField] Transform rigTarget;

    [SerializeField] private float sensitivity = 2.0f; // ���������������� �������� ����

    [Header("Rotate settings")]
    [SerializeField] private float maxYAngleUp = 80.0f;  // ������������ ���� ������ ����� �� ���������
    [SerializeField] private float maxYAngleDown = 80.0f;  // ������������ ���� ������ ���� �� ���������

    public Vector3 input = Vector2.zero;

    [Header("Multiplayer settings")]
    private PhotonView photonView;

    [Header("Synchronization settings")]
    [SerializeField] private Quaternion syncHandsCurrentQuaternion; //��������� ����� ������������� ����� ������������  
    [SerializeField] private Quaternion syncHandsStartQuaternion; //��������� ����� ������������� ����� ������������    
    [SerializeField] private Quaternion syncHandsEndQuaternion; // �������� ����� ������������� �����  ������������

    [SerializeField] private Quaternion syncControllerCurrentQuaternion; //��������� ����� ������������� ����� ������������  
    [SerializeField] private Quaternion syncControllerStartQuaternion; //��������� ����� ������������� ����� ������������    
    [SerializeField] private Quaternion syncControllerEndQuaternion; // �������� ����� ������������� �����  ������������

    [SerializeField] private float lastSynchronizationTime; // ��������� ����� �������������
    [SerializeField] private float syncDelay = 0f; // ������ ����� ������� �������� � ��������� ��������������
    [SerializeField] private float syncTime = 0f; // ����� �������������

    private void Start()
    {
        lastSynchronizationTime = Time.time;
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            //����������������
            mouseX *= sensitivity;
            mouseY *= sensitivity;

            // ���������� �������� ��������
            input.x += mouseX;
            input.y -= mouseY;

            // ����������� ���� �� ��������� 
            input.y = Mathf.Clamp(input.y, -maxYAngleUp, maxYAngleDown);

            // ������� ���������� �� ��� �
            transform.localRotation = Quaternion.Euler(0, input.x, 0);

            // ������� ��� ������ �� ��� �
            playerHands.transform.localRotation = Quaternion.Euler(input.y, 0, 0);
        }
        else
        {
            syncTime += Time.deltaTime;
            float lerpFactor = syncTime / syncDelay;

            syncHandsCurrentQuaternion = Quaternion.Slerp(syncHandsStartQuaternion, syncHandsEndQuaternion, lerpFactor);
            syncHandsCurrentQuaternion.y = 0;
            syncHandsCurrentQuaternion.z = 0;

            syncControllerCurrentQuaternion = Quaternion.Slerp(syncControllerStartQuaternion, syncControllerEndQuaternion, lerpFactor);
            syncControllerCurrentQuaternion.x = 0;
            syncControllerCurrentQuaternion.z = 0;

            playerHands.transform.localRotation = syncHandsCurrentQuaternion;
            transform.localRotation = syncControllerCurrentQuaternion;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Quaternion syncHandsQuaternion;
        Quaternion syncControllerQuaternion;

        if (stream.IsWriting) // ���� ���������� � ����, �� ��������� ������ ������� ����� ���������
        {
            syncHandsQuaternion = playerHands.transform.localRotation;
            syncControllerQuaternion = transform.localRotation;

            stream.SendNext(syncHandsQuaternion);
            stream.SendNext(syncControllerQuaternion);
        }
        else
        {
            syncHandsQuaternion = (Quaternion)stream.ReceiveNext();
            syncControllerQuaternion = (Quaternion)stream.ReceiveNext();

            syncTime = 0f; // ���������� ����� �������������
            syncDelay = Time.time - lastSynchronizationTime; // �������� ������ ���������� �������������
            lastSynchronizationTime = Time.time; // ���������� ����� ����� ��������� �������������

            syncHandsEndQuaternion = syncHandsQuaternion; // ��������  �������� ������������� �����, � ������� ����� ���������� �������� � blend tree
            syncHandsStartQuaternion = playerHands.transform.localRotation; // ��������� �������� �� ��������� � blend tree

            syncControllerEndQuaternion = syncControllerQuaternion; // ��������  �������� ������������� �����, � ������� ����� ���������� �������� � blend tree
            syncControllerStartQuaternion = transform.localRotation; // ��������� �������� �� ��������� � blend tree 
        }
    }
}
