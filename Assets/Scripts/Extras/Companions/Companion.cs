using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour {
    public GameObject player;
    protected Transform playerTransform;
    protected Vector3 velocity = Vector3.zero;

    //change to not be public after testing (place in model file)
    public float maxSpeed;

    void Awake() {
        playerTransform = player.transform;
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }
}
