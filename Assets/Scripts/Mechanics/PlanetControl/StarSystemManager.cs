using System.Collections.Generic;
using UnityEngine;

public class StarSystemManager : MonoBehaviour {

    public StarSystemGenerator starSystemGenerator;
    public Waypoints waypoints;

    void Start() {
        OrbitalDetails orbitalDetails = SaveLoadManager.loadStarSystem();

        GameObject centreMass;

        //generate orbits
        if (orbitalDetails == null) {
            centreMass  = starSystemGenerator.generateStarSystem(gameObject);
            SaveLoadManager.saveStarSystem(getOrbitalDetails(centreMass));
        } else {
            centreMass = starSystemGenerator.loadStarSystem(orbitalDetails);
        }

        waypoints.setCentreMass(centreMass.transform);
    }

    public OrbitalDetails getOrbitalDetails(GameObject baseObject) {
        
        CelestialBody centreMassBody = ((CelestialBody)baseObject.GetComponent(typeof(CelestialBody)));

        OrbitalDetails orbitalDetails = new OrbitalDetails(centreMassBody.radius, centreMassBody.mass, new List<OrbitalDetails>());

        foreach(Transform transform in baseObject.transform) {
            OrbitingBody planetBody = ((OrbitingBody)transform.gameObject.GetComponent(typeof(OrbitingBody)));

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

            foreach (Transform subTransform in transform) {
                OrbitingBody moonBody = ((OrbitingBody)subTransform.gameObject.GetComponent(typeof(OrbitingBody)));

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

        return orbitalDetails;
    }
}
