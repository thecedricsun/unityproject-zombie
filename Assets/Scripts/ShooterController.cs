using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class ShooterController : MonoBehaviour
{
    [SerializeField] private Rig aimRig;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private GameObject rifleCrosshair;
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform bulletProjectile;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Transform vfxHit;

    [Header("Animator Settings")]
    [SerializeField] private int aimLayer;

    [Header("Gun Settings")]
    public int clipSize;
    public int ammoCap;

    [SerializeField] private int currentAmmo;
    [SerializeField] private int reserveAmmo;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private Vector3 mouseWorldPosition; // Declare mouseWorldPosition here
    private float aimRigWeight;
    private float gunHeat;

    private Transform hitTransform;
    [Header("Multipliers")]
    public float damageAmount = 10f;
    public float headshotMultiplier = 2f;
    public float bodyshotMultiplier = 1f;
    public float legshotMultiplier = 0.7f;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        rifleCrosshair.SetActive(false);
        currentAmmo = clipSize;
        reserveAmmo = ammoCap;
    }

    private void Update()
    {
        hitTransform = null;
        ShowScreenCenterPoint();
        OnAim();
        OnFire();
        OnReload();

        if (gunHeat > 0)
        {
            gunHeat -= Time.deltaTime;
        }

        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
    }

    private void ShowScreenCenterPoint()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }
    }

    private void OnAim()
    {
        if (starterAssetsInputs.aim)
        {
            aimRigWeight = 1f;
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(aimLayer, Mathf.Lerp(animator.GetLayerWeight(aimLayer), 1f, Time.deltaTime * 10f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            rifleCrosshair.SetActive(true);
        }
        else
        {
            aimRigWeight = 0f;
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            rifleCrosshair.SetActive(false);
            animator.SetLayerWeight(aimLayer, Mathf.Lerp(animator.GetLayerWeight(aimLayer), 0f, Time.deltaTime * 10f));
        }    
    }

    private void OnFire()
    {
        if (starterAssetsInputs.fire && starterAssetsInputs.aim && currentAmmo > 0 && !animator.GetBool("Reload"))
        {
            if (gunHeat <= 0f)
            {
                gunHeat = 0.20f;
                if (currentAmmo > 5)
                    FindObjectOfType<AudioManager>().Play("PlayerShoot");
                else
                    FindObjectOfType<AudioManager>().Play("PlayerLastShots");
                Vector3 aimDirection = (mouseWorldPosition - bulletSpawnPoint.position).normalized;
                Instantiate(bulletProjectile, bulletSpawnPoint.position, Quaternion.LookRotation(aimDirection, Vector3.up));
                currentAmmo--;
            }
        }
        else
            starterAssetsInputs.fire = false;
    }

    private void OnReload()
    {
        if (starterAssetsInputs.reload && currentAmmo < clipSize && reserveAmmo > 0)
        {
            FindObjectOfType<AudioManager>().Play("PlayerReload");
            animator.SetBool("Reload", true);
            StartCoroutine(ReloadGun());

            int amountNeeded = clipSize - currentAmmo;
            if (amountNeeded >= reserveAmmo)
            {
                currentAmmo += reserveAmmo;
                reserveAmmo -= amountNeeded;
            }
            else
            {
                currentAmmo = clipSize;
                reserveAmmo -= amountNeeded;
            }
        }
    }

    IEnumerator ReloadGun()
    {
        if (animator.GetLayerWeight(2) > 0)
        {
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
        animator.SetBool("Reload", false);
    }

    private float GetDamageMultiplier(string hitboxTag)
    {
        // Determine the damage multiplier based on the hitbox tag
        // For example, if the tag is "Hitbox Head," return the headshot multiplier

        if (hitboxTag.Contains("Head"))
        {
            return headshotMultiplier;
        }
        else if (hitboxTag.Contains("Body"))
        {
            return bodyshotMultiplier;
        }
        else if (hitboxTag.Contains("Leg"))
        {
            return legshotMultiplier;
        }
        else if (hitboxTag.Contains("Arm"))
        {
            return bodyshotMultiplier;
        }

        // Default multiplier if the hitbox tag doesn't match any specific body part
        return 1f;
    }
}
