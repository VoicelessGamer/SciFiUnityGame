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

    private OrbitalDetails orbitalDetails;

    // Start is called before the first frame update
    void Start() {
        orbitalDetails = SaveLoadManager.loadStarSystem();

        //generate orbits
        if(orbitalDetails == null) {
            generateStarSystem();
        } else {
            loadStarSystem();
        }
    }

    public void generateStarSystem() {
        GameObject centreMass = (GameObject)Instantiate(systemCentreMass, new Vector3(0,0,0), Quaternion.identity);
        centreMass.name = "CentreMass";
        CelestialBody centreMassBody = ((CelestialBody)centreMass.GetComponent(typeof(CelestialBody)));

        orbitalDetails = new OrbitalDetails(centreMassBody.radius, centreMassBody.mass, new List<OrbitalDetails>());

        float currentMinBodySeparation = Random.Range(initialMinBodySeparation, initialMaxBodySeparation);

        int totalPlanets = (int)Random.Range(minPlanetGeneration, maxPlanetGeneration);

        for(int i = 0; i < totalPlanets; i++) {
            GameObject planet = (GameObject)Instantiate(planetPrefab, centreMass.transform);
            planet.name = "Planet-" + i;
            OrbitingBody planetBody = ((OrbitingBody)planet.GetComponent(typeof(OrbitingBody)));
            planetBody.minBodySeparation = currentMinBodySeparation;
            planetBody.setupOrbit();

            currentMinBodySeparation = planetBody.getDistanceFromFoci() + minBodySeparation;
            
            OrbitalDetails planetOrbitalDetails = new OrbitalDetails(planetBody.radius, 
                planetBody.mass,
                planetBody.getSemiMinorAxis(),
                planetBody.getSemiMajorAxis(),
                planetBody.getEccentricity(),
                planetBody.getFoci1(),
                planetBody.getFoci2(),
                planetBody.getCentre(),
                planetBody.getLocalCentreVector(),
                planetBody.getCurrentTheta(),
                planetBody.getCosineEllipseRotation(),
                planetBody.getSineEllipseRotation(),
                planetBody.getDistanceFromFoci(),
                new List<OrbitalDetails>());

            //moon generation for current planet
            float currentMinMoonSeparation = Random.Range(initialMinMoonSeparation, initialMaxMoonSeparation);
            
            int totalMoons = (int)Random.Range(minMoonGeneration, maxMoonGeneration);

            for(int j = 0; j < totalMoons; j++) {
                GameObject moon = (GameObject)Instantiate(moonPrefab, planet.transform);
                moon.name = "Planet-" + i + "-Moon-" + j;
                OrbitingBody moonBody = ((OrbitingBody)moon.GetComponent(typeof(OrbitingBody)));
                moonBody.minBodySeparation = currentMinMoonSeparation;
                moonBody.setupOrbit();

                currentMinMoonSeparation = moonBody.getDistanceFromFoci() + minMoonSeparation;
                
                OrbitalDetails moonOrbitalDetails = new OrbitalDetails(moonBody.radius, 
                    moonBody.mass,
                    moonBody.getSemiMinorAxis(),
                    moonBody.getSemiMajorAxis(),
                    moonBody.getEccentricity(),
                    moonBody.getFoci1(),
                    moonBody.getFoci2(),
                    moonBody.getCentre(),
                    moonBody.getLocalCentreVector(),
                    moonBody.getCurrentTheta(),
                    moonBody.getCosineEllipseRotation(),
                    moonBody.getSineEllipseRotation(),
                    moonBody.getDistanceFromFoci(),
                    new List<OrbitalDetails>());

                planetOrbitalDetails.addOrbitingBody(moonOrbitalDetails);                
            }

            orbitalDetails.addOrbitingBody(planetOrbitalDetails);      
        }

        SaveLoadManager.saveStarSystem(orbitalDetails);
    }

    public void loadStarSystem() {
        
        GameObject centreMass = (GameObject)Instantiate(systemCentreMass, new Vector3(0,0,0), Quaternion.identity);
        centreMass.name = "CentreMass";
        CelestialBody centreMassBody = ((CelestialBody)centreMass.GetComponent(typeof(CelestialBody)));
        centreMassBody.loadDetails(orbitalDetails.getRadius(), orbitalDetails.getMass());

        for(int i = 0; i < orbitalDetails.getOrbitingBodies().Count; i++) {
            GameObject planet = (GameObject)Instantiate(planetPrefab, centreMass.transform);
            planet.name = "Planet-" + i;
            OrbitingBody planetBody = ((OrbitingBody)planet.GetComponent(typeof(OrbitingBody)));
            planetBody.loadDetails(orbitalDetails.getOrbitingBodies()[i]);

            //moon generation for current planet
            for(int j = 0; j < orbitalDetails.getOrbitingBodies()[i].getOrbitingBodies().Count; j++) {
                GameObject moon = (GameObject)Instantiate(moonPrefab, planet.transform);
                moon.name = "Planet-" + i + "-Moon-" + j;
                OrbitingBody moonBody = ((OrbitingBody)moon.GetComponent(typeof(OrbitingBody)));
                moonBody.loadDetails(orbitalDetails.getOrbitingBodies()[i].getOrbitingBodies()[j]);
            }   
        }
    }
}
