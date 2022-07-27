using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class ThirdPersonAttackController : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    private bool attacking= false;
    public  bool aiming= false;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform BulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    //[SerializeField] private Transform vfxHitGreen;
    //[SerializeField] private Transform vfxHitRed;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private Pickup pickupscript;
    private AudioSource audioSource;

    public Rig AimRiglayer;
    public GameObject muzzleFlash;
    public GameObject[] weaponpick;

    private void Awake() {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        pickupscript = weaponpick[0].GetComponent<Pickup>();
    }

    private void Update() {
        Attack();
    }

    void Attack()
    {
        var mouse = Mouse.current;

        if (mouse == null)
            return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            if (attacking)
            {
                attacking = false;
            }
            else
            {
                attacking = true;
            }
        }

        //melee attack
        if(!pickupscript.weaponactive)
        {
            if (attacking)
            {
                animator.SetBool("Attack", true);
                attacking = false;
            }
            else
            {
                animator.SetBool("Attack", false);
            }
        }
        //gun shoot
        else
        {
            Vector3 mouseWorldPosition = Vector3.zero;

            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            Transform hitTransform = null;
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                debugTransform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
                hitTransform = raycastHit.transform;
            }

            if (pickupscript.weaponactive)
            {
                if (starterAssetsInputs.aim)
                {
                    aiming = true;
                    aimVirtualCamera.gameObject.SetActive(true);
                    thirdPersonController.SetSensitivity(aimSensitivity);
                    thirdPersonController.SetRotateOnMove(false);
                    animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 1f, Time.deltaTime * 10f));
                    AimRiglayer.weight = Mathf.Lerp(AimRiglayer.weight, 1f, Time.deltaTime * 13f);

                    Vector3 worldAimTarget = mouseWorldPosition;
                    worldAimTarget.y = transform.position.y;
                    Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

                    transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
                }
                else
                {
                    aiming = false;
                    aimVirtualCamera.gameObject.SetActive(false);
                    thirdPersonController.SetSensitivity(normalSensitivity);
                    thirdPersonController.SetRotateOnMove(true);
                    animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 0f, Time.deltaTime * 10f));
                    AimRiglayer.weight = Mathf.Lerp(AimRiglayer.weight, 0f, Time.deltaTime * 13f);
                }
            }

            if (starterAssetsInputs.shoot)
            {
                /*
                // Hit Scan Shoot
                if (hitTransform != null) {
                    // Hit something
                    if (hitTransform.GetComponent<BulletTarget>() != null) {
                        // Hit target
                        Instantiate(vfxHitGreen, mouseWorldPosition, Quaternion.identity);
                    } else {
                        // Hit something else
                        Instantiate(vfxHitRed, mouseWorldPosition, Quaternion.identity);
                    }
                }
                //
                //*/
                // Projectile Shoot
                Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                Instantiate(BulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

                audioSource.enabled = true;
                audioSource.Play();
                muzzleFlash.SetActive(true);
                StartCoroutine(wait());

                starterAssetsInputs.shoot = false;
            }
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }
}