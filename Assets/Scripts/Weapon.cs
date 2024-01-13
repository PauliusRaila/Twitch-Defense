using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public GameObject bulletPrefab;
    public Transform fireSlot;
    public playerController owner;
    //public enum weaponTypes { sharpshooter, soldier, scientist, demolition}
    //[SerializeField]
    // public weaponTypes weaponType;

    public float fireRate = 1f;
    public float shootRadius = 1f;
    public float weaponDamage = 1f;
    public float critRate = 0f;
    public float explosionRadius = 0f;
    private float fireCountdown = 1f;


    public void Shoot(Transform target) {
        if (fireCountdown <= 0f)
        {
            GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, fireSlot.position, fireSlot.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();

            if (bullet != null) {
                bullet.explosionRadius = explosionRadius;
                bullet.damage = weaponDamage;
                bullet.Seek(target);
            }
                

            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    
}
