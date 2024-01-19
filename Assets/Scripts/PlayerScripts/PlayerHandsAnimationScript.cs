using Photon.Pun;
using UnityEngine;

public class PlayerHandsAnimationScript : MonoBehaviour
{
    [SerializeField] private Animator playerHandsAnimator;
    private PhotonView photonView;

    private void OnEnable()
    {
        transform.root.GetComponent<PlayerMoveScript>().onIdle += StartIdleAnimation;
        transform.root.GetComponent<PlayerMoveScript>().onWalk += StartWalkAnimation;
        transform.root.GetComponent<PlayerMoveScript>().onRun += StartRunAnimation;
    }

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void GetHitAnimation()
    {
        playerHandsAnimator.SetTrigger("GetHit");
    }

    public void StartReloadAnimation()
    {
        playerHandsAnimator.SetTrigger("Reload");
    }

    public void StartShootAnimation()
    {
        playerHandsAnimator.SetTrigger("Shoot");
    }

    public void StartWalkAnimation()
    {
        playerHandsAnimator.SetBool("IsRun", false);
        playerHandsAnimator.SetBool("IsWalk", true);
    }

    public void StartRunAnimation()
    {
        playerHandsAnimator.SetBool("IsWalk", false);
        playerHandsAnimator.SetBool("IsRun", true);
    }

    public void StartIdleAnimation()
    {
        playerHandsAnimator.SetBool("IsWalk", false);
        playerHandsAnimator.SetBool("IsRun", false);
    }

    public void StartJumpAnimation()
    {
        playerHandsAnimator.SetBool("IsWalk", false);
        playerHandsAnimator.SetBool("IsRun", false);
    }

    public void EnableApplyRootMotion()
    {
        playerHandsAnimator.applyRootMotion = true;
    }

    public void DisableApplyRootMotion()
    {
        playerHandsAnimator.applyRootMotion = false;
    }

    // ����� ��� ��������, ������������� �� �������� ������
    public bool IsAnimationPlaying(string name)
    {

        // �������� ���������� � ������� ��������� ���������
        AnimatorStateInfo stateInfo = playerHandsAnimator.GetCurrentAnimatorStateInfo(0);

        // ���������� ��� ������� �������� � ������� ������
        if (stateInfo.IsName(name))
        {
            return true;
        }

        return false; // �������� �� ������������� ������
    }
}
