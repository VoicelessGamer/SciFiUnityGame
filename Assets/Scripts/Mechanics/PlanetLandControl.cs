using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetLandControl : MonoBehaviour {

    private bool sceneChangeAvailable = false;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            sceneChangeAvailable = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            sceneChangeAvailable = false;
        }
    }

    void Update() {
        if(sceneChangeAvailable && Input.GetKeyDown(KeyCode.X)) {
            SceneManager.LoadScene(1);
        }
    }
}
