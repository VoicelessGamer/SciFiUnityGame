using Platformer.Mechanics;
using UnityEngine;

public class EnemyController : KinematicObject {

    public Transform playerTransform;

    public float movementSpeed = 4f;

    public float maxDistanceFromPlayer = 4f;
    
    public bool controlEnabled = true;

    private Rigidbody2D rigidBody;
    private Health health;
    private SpriteRenderer spriteRenderer;
    private Vector2 move;

    void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected override void Update() {
        if(controlEnabled && Vector2.Distance(transform.position, playerTransform.position) > maxDistanceFromPlayer) {
            if (transform.position.x < playerTransform.position.x) {
                //player is to the right of this enemy
                move.x = 1;
            } else {
                //player is to the left of this enemy
                move.x = -1;
            }
        } else {
            move.x = 0;
        }
        base.Update();
    }
    protected override void ComputeVelocity() {
        targetVelocity = move * movementSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Projectile")) {
            health.dealDamage(collision.gameObject.GetComponent<Projectile>().damage);

            if (health.isDead) {
                die();
            }
        }
    }

    private void die() {
        Destroy(gameObject);
    }
}
