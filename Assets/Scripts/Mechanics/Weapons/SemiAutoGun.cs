using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutoGun : Gun {
    
    // Update is called once per frame
    void Update() {
        if (Time.time >= cooldown) {
            if (Input.GetButtonDown("Fire1")) {
                attack();
            }
        }
    }
}
