using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : HoldibleItem{
    [SerializeField]
    private float AutoFireRate;

    private bool fireing;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject bulletPrefab, muzzleFlashPrefab;
    [SerializeField]
    private Transform barrelLocation;

    [SerializeField]
    private float shotPower = 750f;

    public override void OnInteract() {
        fireing = true;
        if (Player.Instance.GunFireModeAuto) {
            StartCoroutine(AutoFire());
        }
        else {
            Fire();
        }
    }

    public override void OnStopInteract() {
        fireing = false;
    }

    private void Fire() {
        if (Player.Instance.Ammo > 0) {
            Player.Instance.Ammo--;
            animator.SetTrigger("Fire");
        }
    }

    private IEnumerator AutoFire() {
        float lastShotTime = 0;
        while (fireing) {
            if (lastShotTime + (1 / AutoFireRate) <= Time.time) {
                lastShotTime = Time.time;
                Fire();
            }
            yield return null;
        }
    }

    void Shoot()
    {
        GameObject tempFlash, bullet;
        bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
        tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

        Destroy(tempFlash, 0.5f);
        Destroy(bullet, 3f);       
    }
}
