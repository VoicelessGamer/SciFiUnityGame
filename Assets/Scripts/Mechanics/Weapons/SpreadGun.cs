using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadGun : Gun {

    public int pellets;

    // Update is called once per frame
    void Update() {
        if (Time.time >= cooldown) {
            if (Input.GetButtonDown("Fire1")) {
                for(int i = 0; i < pellets; i++) {
                    attack();
                }
            }
        }
    }
}
