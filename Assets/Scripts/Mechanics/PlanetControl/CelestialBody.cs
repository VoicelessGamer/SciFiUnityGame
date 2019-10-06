using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour {
    [Header("Planetary Details")]
    public float radius;
    public float mass;

    //modified constant (Real: 0.0000000000667408f)
    protected float gravitationalConstant = 0.00000667408f;

    public void loadDetails(float radius, float mass) {
        this.radius = radius;
        this.mass = mass;
    }
}
