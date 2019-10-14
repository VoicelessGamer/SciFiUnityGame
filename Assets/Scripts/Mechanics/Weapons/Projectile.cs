using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float lifeTime = 2.0f;
    private float deathTime;
    public int damage;

    void Start() {
        deathTime = Time.time + lifeTime;
    }

    void Update() {
        if(Time.time >= deathTime) {
            destroyProjectile();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(!collision.gameObject.CompareTag("Projectile")) {
            destroyProjectile();
        }
    }

    void destroyProjectile() {
        Destroy(gameObject);
    }
}
