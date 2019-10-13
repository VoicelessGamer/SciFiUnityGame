using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Weapon {
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float roundsPerMinute;
    public Transform bulletSpawnPoint;
    [Range(1,100)]
    public float accuracy;

    protected const float maxDeviationAngle = 15.0f;
    private float deviationAngle;
    private float fireRate;

    void Start() {
        deviationAngle = accuracy >= 100 ? 0 : maxDeviationAngle * ((100 - accuracy) / 100);
        fireRate = 1 / (roundsPerMinute / 60);
    }

    protected Quaternion getInitialRotation() {
        Vector3 mouse = Input.mousePosition;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        
        //angle directly pointing at the mouse pointer
        float centredAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        //alter trajectory based on accuracy
        float actualAngle = Random.Range(centredAngle - deviationAngle, centredAngle + deviationAngle);

        return Quaternion.Euler(0, 0, actualAngle);
    }

    protected override void attack() {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, getInitialRotation());

        bullet.GetComponent<Projectile>().damage = damage;

        Rigidbody2D rBody = (Rigidbody2D)bullet.GetComponent(typeof(Rigidbody2D));

        rBody.AddForce(bullet.transform.right * bulletSpeed);

        cooldown = Time.time + fireRate;
    }
}
