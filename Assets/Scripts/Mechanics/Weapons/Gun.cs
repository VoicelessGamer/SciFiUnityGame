using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon {
    public int damage;
    public GameObject bulletPrefab;
    public float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, getInitialRotation());
        }
    }
}
