using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystemGenerator : MonoBehaviour
{
    public GameObject systemCentreMass;
    public GameObject planetPrefab;
    public GameObject moonPrefab;
    public int minPlanetGeneration;
    public int maxPlanetGeneration;
    public int minMoonGeneration;
    public int maxMoonGeneration;

    [Header("Planetary Body Variables")]
    //minBodySeparation - This value plus the sum of the radii of this body and the orbiting body is the minimum 
    //distance between the two bodies when at periapsis
    public float minBodySeparation;
    public float initialMinBodySeparation;
    public float initialMaxBodySeparation;
    
    [Header("Moon Body Variables")]
    public float minMoonSeparation;
    public float initialMinMoonSeparation;
    public float initialMaxMoonSeparation;

    // Start is called before the first frame update
    void Start() {
        
        GameObject centreMass = (GameObject)Instantiate(systemCentreMass, new Vector3(0,0,0), Quaternion.identity);

        float currentminBodySeparation = Random.Range(initialMinBodySeparation, initialMaxBodySeparation);

        int totalPlanets = (int)Random.Range(minPlanetGeneration, maxPlanetGeneration);

        for(int i = 0; i < totalPlanets; i++) {
            GameObject planet = (GameObject)Instantiate(planetPrefab, centreMass.transform);
            planet.name = "Planet" + i;
            OrbitingBody planetBody = ((OrbitingBody)planet.GetComponent(typeof(OrbitingBody)));
            planetBody.minBodySeparation = currentminBodySeparation;
            planetBody.setupOrbit();

            currentminBodySeparation = planetBody.getDistanceFromFoci() + minBodySeparation;

            //moon generation for current planet
            float currentminMoonSeparation = Random.Range(initialMinMoonSeparation, initialMaxMoonSeparation);
            
            int totalMoons = (int)Random.Range(minMoonGeneration, maxMoonGeneration);

            for(int j = 0; j < totalMoons; j++) {
                GameObject moon = (GameObject)Instantiate(moonPrefab, planet.transform);
                moon.name = "Moon" + j;
                OrbitingBody moonBody = ((OrbitingBody)moon.GetComponent(typeof(OrbitingBody)));
                moonBody.minBodySeparation = currentminMoonSeparation;
                moonBody.setupOrbit();
                
                currentminMoonSeparation = moonBody.getDistanceFromFoci() + minMoonSeparation;
            }
        }
    }
}
