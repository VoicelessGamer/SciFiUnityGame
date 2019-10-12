using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float lifeTime = 2.0f;
    private float deathTime;

    void Start() {
        deathTime = Time.time + lifeTime;
    }

    void Update() {
        if(Time.time >= deathTime) {
            destroyBullet();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(!collision.gameObject.CompareTag("Bullet")) {
            destroyBullet();
        }
    }

    void destroyBullet() {
        Destroy(gameObject);
    }
}
