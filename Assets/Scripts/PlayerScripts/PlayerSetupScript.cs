using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerSetupScript : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera weaponCamera;

    [SerializeField] private GameObject playerHands;
    [SerializeField] private GameObject playeSkin;

    [SerializeField] private string nickname;

    [Header("Scripts settings")]
    [SerializeField] private PlayerMoveScript playerMoveScript;
    [SerializeField] private PlayerHandsAnimationScript playerHandsAnimationScript;
    [SerializeField] private PlayerBodyAnimationScript playerBodyAnimationScript;
    [SerializeField] private PlayerMouseLookScript playerMouseLookScript;
    [SerializeField] private PlayerHealthScript playerHealthScript;
    [SerializeField] private PlayerShooterScript playerShooterScript;

    [Header("UI settings")]
    [SerializeField] private GameObject playerStatsCanvas;
    [SerializeField] private GameObject playerNickNameCanvas;
    [SerializeField] private TMP_Text playerNicknameText;

    [SerializeField] private GameObject scopeImage;

    private void OnEnable()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            GamePopupMenuManagerScript.onPopupMenuAppear += SetupForMenuViewing;
            GamePopupMenuManagerScript.onPopupMenuDisappear += SetupForCloseMenu;

            GameManagerScript.onGameEnd += PlayerDeathSetup;
        }
    }

    private void OnDisable()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            GamePopupMenuManagerScript.onPopupMenuAppear -= SetupForMenuViewing;
            GamePopupMenuManagerScript.onPopupMenuDisappear -= SetupForCloseMenu;

            GameManagerScript.onGameEnd -= PlayerDeathSetup;
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void IsLocalPlayer()
    {
        playerCamera.gameObject.SetActive(true);
        playerStatsCanvas.SetActive(true);
        playerNickNameCanvas.SetActive(false);
        EnablePlayerScripts();
        ChangePlayerLayersToLocalPlayer();
    }

    public void PlayerDeathSetup()
    {
        DisablePlayerScripts();
        ChangePlayerLayersToAnotherPlayer();

        GetComponent<CharacterController>().enabled = false;
        playerNickNameCanvas.SetActive(false);
        playerStatsCanvas.SetActive(false);

    }

    private void EnablePlayerScripts()
    {
        playerMoveScript.enabled = true;
        playerHandsAnimationScript.enabled = true;
        playerBodyAnimationScript.enabled = true;
        playerMouseLookScript.enabled = true;
        playerHealthScript.enabled = true;
        playerShooterScript.enabled = true;
    }

    private void DisablePlayerScripts()
    {
        playerMoveScript.enabled = false;
        playerHandsAnimationScript.enabled = false;
        playerBodyAnimationScript.enabled = false;
        playerMouseLookScript.enabled = false;
   
        playerShooterScript.enabled = false;
    }

    private void ChangePlayerLayersToLocalPlayer()
    {
        //��������� ���� ��������� ��� � ���� ��� ���������� ������
        ChangeLayerRecursivelyTo(playerHands.transform, "LocalPlayerHands");
        ChangeLayerRecursivelyTo(playeSkin.transform, "LocalPlayerBody");
    }

    private void ChangePlayerLayersToAnotherPlayer()
    {
        DisableLayerByName(weaponCamera, "LocalPlayerHands");
        EnableLayerByName(playerCamera, "LocalPlayerBody");
    }

    public void SetupForMenuViewing()
    {
        playerMoveScript.enabled = false;
        playerMouseLookScript.enabled = false;
        playerShooterScript.enabled = false;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        scopeImage.SetActive(false);
        StartCoroutine(CameraPositionSetup(-0.237f, -0.52f, -0.386f));
    }

    public void SetupForCloseMenu()
    {
        playerMoveScript.enabled = true;
        playerMouseLookScript.enabled = true;
        playerShooterScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        scopeImage.SetActive(true);
    }

    [PunRPC]
    public void SetNickname(string nickname)
    {
        this.nickname = nickname;
        playerNicknameText.text = this.nickname;
    }

    // ����������� ������� ��� ��������� ���� ������� � ��� �������� ���������
    private void ChangeLayerRecursivelyTo(Transform targetTransform, string layerName)
    {
        // �������� ���� �������� �������
        targetTransform.gameObject.layer = LayerMask.NameToLayer(layerName);

        // �������� �� ���� �������� ��������� �������� �������
        foreach (Transform child in targetTransform)
        {
            // ���������� �������� ������� ��� ��������� ���� ��������� ��������
            ChangeLayerRecursivelyTo(child, layerName);
        }
    }

    // ����� ��� ��������� ���� �� ��������
    private void DisableLayerByName(Camera camera, string layerName)
    {
        int layerIndex = LayerMask.NameToLayer(layerName);

        if (layerIndex != -1)
        {
            camera.cullingMask &= ~(1 << layerIndex);
        }
        else
        {
            Debug.LogWarning("Layer with name " + layerName + " not found.");
        }
    }

    private void EnableLayerByName(Camera camera, string layerName)
    {
        int layerIndex = LayerMask.NameToLayer(layerName);

        if (layerIndex != -1)
        {
            camera.cullingMask |= 1 << layerIndex;
        }
        else
        {
            Debug.LogWarning("Layer with name " + layerName + " not found.");
        }
    }

    private IEnumerator CameraPositionSetup(float x, float y, float z)
    {
        Vector3 initialPosition = playerCamera.transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < 0.75f) // ��������� �������� ��  1 �������
        {
            playerCamera.transform.localPosition = Vector3.Lerp(initialPosition, new Vector3(x, y, z), elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null; // ��������� ����
        }
    }
}


