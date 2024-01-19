using Photon.Pun;
using UnityEngine;

public class PlayerMouseLookScript : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("General settings")]
    [SerializeField] private Camera playerCamera;      // камера игрока
    [SerializeField] private GameObject playerHands;      //Руки игрока  

    [SerializeField] Transform rigTarget;

    [SerializeField] private float sensitivity = 2.0f; // Чувствительность движения мыши

    [Header("Rotate settings")]
    [SerializeField] private float maxYAngleUp = 80.0f;  // Максимальный угол сдвига вверх по вертикали
    [SerializeField] private float maxYAngleDown = 80.0f;  // Максимальный угол сдвига вниз по вертикали

    public Vector3 input = Vector2.zero;

    [Header("Multiplayer settings")]
    private PhotonView photonView;

    [Header("Synchronization settings")]
    [SerializeField] private Quaternion syncHandsCurrentQuaternion; //начальное число вертикального ввода интерполяции  
    [SerializeField] private Quaternion syncHandsStartQuaternion; //начальное число вертикального ввода интерполяции    
    [SerializeField] private Quaternion syncHandsEndQuaternion; // конечное число вертикального ввода  интерполяции

    [SerializeField] private Quaternion syncControllerCurrentQuaternion; //начальное число вертикального ввода интерполяции  
    [SerializeField] private Quaternion syncControllerStartQuaternion; //начальное число вертикального ввода интерполяции    
    [SerializeField] private Quaternion syncControllerEndQuaternion; // конечное число вертикального ввода  интерполяции

    [SerializeField] private float lastSynchronizationTime; // последнее время синхронизации
    [SerializeField] private float syncDelay = 0f; // дельта между текущим временем и последней синхронизацией
    [SerializeField] private float syncTime = 0f; // время синхронизации

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

            //чувствительность
            mouseX *= sensitivity;
            mouseY *= sensitivity;

            // Обновление текущего поворота
            input.x += mouseX;
            input.y -= mouseY;

            // Ограничение угла по вертикали 
            input.y = Mathf.Clamp(input.y, -maxYAngleUp, maxYAngleDown);

            // Поворот коллайдера по оси х
            transform.localRotation = Quaternion.Euler(0, input.x, 0);

            // Поворот рук игрока по оси у
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

        if (stream.IsWriting) // если отправляем в сеть, то считываем данные объекта перед отправкой
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

            syncTime = 0f; // сбрасываем время синхронизации
            syncDelay = Time.time - lastSynchronizationTime; // получаем дельту предыдущей синхронизации
            lastSynchronizationTime = Time.time; // записываем новое время последней синхронизации

            syncHandsEndQuaternion = syncHandsQuaternion; // конечная  величина вертикального ввода, к которой будет стримиться анимация в blend tree
            syncHandsStartQuaternion = playerHands.transform.localRotation; // начальная величина по вертикали в blend tree

            syncControllerEndQuaternion = syncControllerQuaternion; // конечная  величина вертикального ввода, к которой будет стримиться анимация в blend tree
            syncControllerStartQuaternion = transform.localRotation; // начальная величина по вертикали в blend tree 
        }
    }
}
