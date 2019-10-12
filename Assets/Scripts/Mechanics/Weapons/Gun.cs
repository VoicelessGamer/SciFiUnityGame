using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon {
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float fireRate;
    public Transform bulletSpawnPoint;
    [Range(0,100)]
    public float accuracy;

    // Update is called once per frame
    void Update() {
        if(Time.time >= cooldown) {
            if(Input.GetButtonDown("Fire1")) {
                attack();
            }
         }
    }

    protected override void attack() {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, getInitialRotation());

        Rigidbody2D rBody = (Rigidbody2D)bullet.GetComponent(typeof(Rigidbody2D));

        rBody.AddForce(transform.right * bulletSpeed);

        cooldown = Time.time + fireRate;
    }
}
