using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoint : MonoBehaviour {

    public Transform playerTransform;
    
    // Update is called once per frame
    void Update() {
        Vector3 offset = new Vector2(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}