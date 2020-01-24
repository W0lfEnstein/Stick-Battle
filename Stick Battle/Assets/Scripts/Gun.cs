using UnityEngine;
using System.Collections;

[AddComponentMenu("Gun")]
public class Gun : MonoBehaviour
{
    [Header("Gun Info")]

    public string GunId;
    public string GunName;
    public GunType gunType;

    [Header("Gun Settings")]

    public float damage;
    public float distance;

    public float shotInterval;
    public float reloadTime;

    public int magCapacity;
    public int maxAmmo;


    [Header("Current Ammo Info")]
    public int curMagAmount;
    public int curDeductedAmount;
    public int curAmount;

    private bool isReloading;

    private Animator anim;
    private AudioSource audioSource;

    private Transform rayOrigin;

    private void Start()
    {
        rayOrigin = GetComponentInParent<PlayerController>().ThirdPersonCamera.transform;

        curDeductedAmount = curAmount - curMagAmount;
    }

    private void Shoot()
    {
        if(curMagAmount == 0)
        {
            Debug.Log("No Ammo!");
            return;
        }

        Debug.Log("Shot!");

        RaycastHit hit;
        if (Physics.Raycast(new Ray(rayOrigin.position, rayOrigin.forward), out hit, distance))
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.forward * distance, Color.red, 100);
            var target = hit.transform.GetComponent<IDamagable>();
            if (target != null)
                target.TakeDamage(damage);
        }

        curMagAmount--;
        curAmount--;

        StartCoroutine(Reloading(shotInterval));
    }

    private void Reload()
    {
        if (curMagAmount == magCapacity)
        {
            Debug.Log("Full Ammo!");
            return;
        }
        if (curDeductedAmount == 0)
        {
            Debug.Log("No Ammo!");
            return;
        }

        int ammo = ((magCapacity - curMagAmount) < curDeductedAmount) ? (magCapacity - curMagAmount) : curDeductedAmount;

        StartCoroutine(Reloading(reloadTime, ammo));
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !isReloading)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            Reload();
        }
    }


    private IEnumerator Reloading(float duration, int ammo = 0)
    {
        isReloading = true;

        yield return new WaitForSeconds(duration);

        curMagAmount += ammo;
        curDeductedAmount = curAmount - curMagAmount;

        isReloading = false;
    }
}

public enum GunType
{
    Pistol,
    Revolver,
    Rifle,
    Shotgun,
    SMG,
    AssaultRifle,
    SniperRifle,
    MachineGun
}

