using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullyAutoGun : Gun {

    // Update is called once per frame
    void Update() {
        if (Time.time >= cooldown) {
            if (Input.GetButton("Fire1")) {
                attack();
            }
        }
    }
}
