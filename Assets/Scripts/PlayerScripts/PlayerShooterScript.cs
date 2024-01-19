using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooterScript : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private PlayerHandsAnimationScript handsAnimationController;

    void Update()
    {
        if (Input.GetMouseButton(0)
            && !handsAnimationController.IsAnimationPlaying("Reload")
            && !handsAnimationController.IsAnimationPlaying("Run")
            && !handsAnimationController.IsAnimationPlaying("SwitchPoseToRun")
            && !handsAnimationController.IsAnimationPlaying("SwitchPoseToNormal"))
        {
            currentWeapon.Shoot(playerCamera);
        }
        currentWeapon.Recovering();
    }
}
