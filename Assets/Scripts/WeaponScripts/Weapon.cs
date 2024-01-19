using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField] protected Transform playerController;
    [SerializeField] protected Transform playerHandsTransform;
    [SerializeField] protected Transform bulletSpawn;

    protected Vector3 handsOriginalPosition;

    [Header("Fire settings")]
    //базовый урон оружия
    protected float damage;
    //расстояния попадания оружия
    protected float range;
    //урон который должен пройти по игроку, после попадания по разным припяствиям
    protected float currentDamage;

    [SerializeField] protected float fireRate;
    protected float nextFire;

    [Header("Fireability settings")]
    //коэффициент влияющий на урон от оружия при попадании по легкому металлу
    protected float lightMetallСoefficient;
    //коэффициент при попадании по среднему металлу
    protected float mediumMetallСoefficient;
    //коэффициент при попадании по тяжелому металлу
    protected float heavyMetallСoefficient;

    //коэффициент при попадании по лестнице
    protected float ladderСoefficient;

    //коэффициент при попадании по голове игрока
    protected float playerHeadСoefficient;
    //коэффициент при попадании по телу игрока
    protected float playerBodyСoefficient;
    //коэффициент при попадании по руке игрока
    protected float playerArmСoefficient;
    //коэффициент при попадании по кисти игрока
    protected float playerHandСoefficient;
    //коэффициент при попадании по ноге игрока
    protected float playerLegСoefficient;
    //коэффициент при попадании по стопе игрока
    protected float playerFootСoefficient;

    [Header("Ray settings")]
    int layerMaskToIgnore;

    [Header("Ammo settings")]
    //количестова магазинов у оружия
    protected int mag;

    protected int ammo;
    protected int magAmmo;

    [Header("Recoil settings")]
    [Range(0, 1)]
    [SerializeField] protected float recoilPercent = 1;

    [Range(0, 1)]
    [SerializeField] protected float recoverPercent = 1;

    [Space]

    [SerializeField] protected float recoilBack;

    [SerializeField] protected float recoilVerticalDown;
    [SerializeField] protected float recoilVerticalUp;

    [SerializeField] protected float recoilHorizonLeft;
    [SerializeField] protected float recoilHorizonRight;

    protected float recoilLength;
    protected float recoverLength;

    [Header("Cosmetic settings")]
    [SerializeField] protected ParticleSystem muzzleFlash;

    [SerializeField] protected AudioClip shotSFX;

    [SerializeField] protected AudioSource GunAudioSource;

    [Header("UI")]
    [SerializeField] protected TMP_Text magText;
    [SerializeField] protected TMP_Text ammoText;

    public void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;

        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }
    public void Shoot(Camera playerCamera)
    {
        //луч выстрела попадает по всем слоям кроме PlayerController и LocalPlayerBody
        layerMaskToIgnore |= 1 << 7;

        //обновление текущего урона от выстрела
        currentDamage = damage;

        if ((Time.time > nextFire) && (ammo > 0))
        {
            Recoil();

            nextFire = Time.time + 1f / fireRate;

            GetComponent<PhotonView>().RPC("PlayShotEffects", RpcTarget.AllBuffered);

            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

            Ray ray = playerCamera.ScreenPointToRay(screenCenter);

            RaycastHit[] hits = Physics.RaycastAll(ray, range, ~layerMaskToIgnore);

            //Сортировка по расстоянию
            hits = hits.ToDictionary(hit => hit, hit => (ray.origin - hit.point).magnitude).OrderBy(pair => pair.Value).Select(pair => pair.Key).ToArray();
                   
            foreach (RaycastHit hit in hits)
            {
                Collider hitCollider = hit.collider;

                Vector3 hitNormal = hit.normal;

                //проверка на то, куда попал луч
                switch (hitCollider.tag)
                {
                    case "LightMetall":
                        currentDamage -= lightMetallСoefficient;
                        break;
                    case "MediumMetall":
                        currentDamage -= mediumMetallСoefficient;
                        break;
                    case "HeavyMetall":
                        currentDamage -= heavyMetallСoefficient;
                        break;
                    case "Ladder":
                        currentDamage -= ladderСoefficient;
                        break;
                    case "PlayerHead":
                        InflictingDamage(hitCollider, hitNormal, currentDamage, playerHeadСoefficient);
                        break;
                    case "PlayerBody":
                        InflictingDamage(hitCollider, hitNormal, currentDamage, playerBodyСoefficient);
                        break;
                    case "PlayerArm":
                        InflictingDamage(hitCollider, hitNormal, currentDamage, playerArmСoefficient);
                        break;
                    case "PlayerHand":
                        InflictingDamage(hitCollider, hitNormal, currentDamage, playerHandСoefficient);
                        break;
                    case "PlayerLeg":
                        InflictingDamage(hitCollider, hitNormal, currentDamage, playerLegСoefficient);
                        break;
                    case "PlayerFoot":
                        InflictingDamage(hitCollider, hitNormal, currentDamage, playerFootСoefficient);
                        break;
                }

                if (damage <= 0)
                {
                    break;
                }             
            }

            ammo--;

            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo;
        }

        //перезарядка, если патроны закончились
        if (ammo <= 0)
        {
            Reload();
        }
    }

    [PunRPC]
    protected void PlayShotEffects()
    {
        GunAudioSource.PlayOneShot(shotSFX);

        muzzleFlash.Play();
    }

    //перезарядка
    private void Reload()
    {
        if (mag > 0)
        {
            mag--;

            ammo = magAmmo;

            playerHandsTransform.GetComponent<PlayerHandsAnimationScript>().StartReloadAnimation();
        }
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }

    //отдача у оружия
    private void Recoil()
    {

        playerController.GetComponent<PlayerMouseLookScript>().input.x -= Random.Range(-recoilHorizonRight, recoilHorizonLeft) * recoilPercent;
        playerController.GetComponent<PlayerMouseLookScript>().input.y -= Random.Range(-recoilVerticalDown, recoilVerticalUp) * recoilPercent;
        playerHandsTransform.localPosition = Vector3.Lerp(playerHandsTransform.localPosition, new Vector3(handsOriginalPosition.x, handsOriginalPosition.y, Mathf.Clamp(playerHandsTransform.localPosition.z - recoilBack, -0.13f, 0.06f)), recoilPercent);
    }

    //восстановление позиции после отдачи
    public void Recovering()
    {
        playerHandsTransform.localPosition = Vector3.Lerp(playerHandsTransform.localPosition, handsOriginalPosition, recoverPercent);
    }

    //Нанесение урона
    public void InflictingDamage(Collider hitCollider, Vector3 hitNormal, float damage, float damageCoefficient)
    {
        damage *= damageCoefficient;

        if (hitCollider.transform.root.GetComponent<PlayerHealthScript>().isDeath == false)
        {
            if (damage >= hitCollider.transform.root.GetComponent<PlayerHealthScript>().playerHealth)
            {
                //вызов метода у всех игроков, отвечающего за пуш уведомления об убийстве игрока
                transform.root.GetComponent<PhotonView>().RPC("PushKillNotification", RpcTarget.AllBuffered, PhotonNetwork.NickName, hitCollider.transform.root.GetComponent<PhotonView>().Owner.NickName);

                PhotonNetwork.LocalPlayer.AddScore(20);
            }

            hitCollider.transform.root.GetComponent<PhotonView>().RPC("GetHit", RpcTarget.AllBuffered, Mathf.RoundToInt(damage), hitCollider.tag, hitNormal);
        }

    }
}

