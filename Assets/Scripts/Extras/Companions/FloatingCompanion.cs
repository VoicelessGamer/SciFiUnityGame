using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCompanion : Companion {
    public float smoothTime;
    private SpriteRenderer playerSpriteRenderer;

    private bool plDir;

    // Start is called before the first frame update
    void Start() {
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() {
        plDir = playerSpriteRenderer.flipX;

        Vector3 targetPosition = new Vector3(playerTransform.position.x + (plDir == true ? 2 : -2), 
            playerTransform.position.y + 2, playerTransform.position.z);
        //Vector3 targetPosition = new Vector3(playerTransform.position.x, playerTransform.position.y + 2, playerTransform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, maxSpeed);
    }
}
