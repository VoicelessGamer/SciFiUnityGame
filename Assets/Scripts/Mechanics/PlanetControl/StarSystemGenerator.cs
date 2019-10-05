using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystemGenerator : MonoBehaviour
{
    public GameObject systemCentreMass;
    public GameObject planetPrefab;
    public GameObject moonPrefab;
    public int totalPlanets;

    // Start is called before the first frame update
    void Start() {
        
        GameObject centreMass = (GameObject)Instantiate(systemCentreMass, new Vector3(0,0,0), Quaternion.identity);

        for(int i = 0; i < this.totalPlanets; i++) {
            GameObject planet = (GameObject)Instantiate(planetPrefab, centreMass.transform);
            OrbitingBody planetBody = ((OrbitingBody)planet.GetComponent(typeof(OrbitingBody)));
            planetBody.setupOrbit();
            
            GameObject moon = (GameObject)Instantiate(moonPrefab, planet.transform);
            OrbitingBody moonBody = ((OrbitingBody)moon.GetComponent(typeof(OrbitingBody)));
            moonBody.setupOrbit();
        }
    }
}
