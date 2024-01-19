using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    public void Start()
    {
        handsOriginalPosition = playerHandsTransform.transform.localPosition;

        //fire settings
        damage = 10;
        range = 100;
        fireRate = 11;
        nextFire = 1;

        //fireability settings
        lightMetall—oefficient = -3;
        mediumMetall—oefficient = -6;
        heavyMetall—oefficient = -10;

        ladder—oefficient = -5;

        playerHead—oefficient = 10;
        playerBody—oefficient = 1;
        playerArm—oefficient = 0.5f;
        playerHand—oefficient = 0.2f;
        playerLeg—oefficient = 0.6f;
        playerFoot—oefficient = 0.2f;

        //mag settings
        mag = 5;
        ammo = 30;
        magAmmo = 30;
    }

}
