using Platformer.Mechanics;
using UnityEngine;

public class EnemyController : KinematicObject {

    public Transform playerTransform;

    public float movementSpeed = 4f;

    public float jumpTakeOffSpeed = 7;

    public float maxDistanceFromPlayer = 4f;
    
    public bool controlEnabled = true;

    public Collider2D collider2d;

    private Rigidbody2D rigidBody;
    private Health health;
    private SpriteRenderer spriteRenderer;
    private Vector2 move;
    private bool jump;
    public Bounds bounds => collider2d.bounds;

    void Awake() {
        collider2d = GetComponent<Collider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected override void Update() {
        jump = false;
        if (controlEnabled && Vector2.Distance(transform.position, playerTransform.position) > maxDistanceFromPlayer) {
            RaycastHit2D hit;
            if (transform.position.x < playerTransform.position.x) {
                //player is to the right of this enemy
                move.x = 1;
                hit = Physics2D.Raycast(new Vector3(bounds.max.x + 0.01f, bounds.min.y, gameObject.transform.position.z), Vector2.right, 3f);
            } else {
                //player is to the left of this enemy
                move.x = -1;
                hit = Physics2D.Raycast(new Vector3(bounds.min.x - 0.01f, bounds.min.y, gameObject.transform.position.z), Vector2.left, 3f);
            }
            if(hit.collider is CompositeCollider2D) {
                jump = true;
            }
        } else {
            move.x = 0;
        }
        base.Update();
    }
    protected override void ComputeVelocity() {
        if (jump && IsGrounded) {
            move.y = jumpTakeOffSpeed * 1.5f;
            jump = false;
        }
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
