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
            Orbit planetOrbit = ((Orbit)planet.GetComponent(typeof(Orbit)));
            planetOrbit.setupOrbit();
            
            GameObject moon = (GameObject)Instantiate(moonPrefab, planet.transform);
            Orbit moonOrbit = ((Orbit)moon.GetComponent(typeof(Orbit)));
            moonOrbit.setupOrbit();
        }
    }
}
