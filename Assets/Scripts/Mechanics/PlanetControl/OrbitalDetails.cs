using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalDetails {
    private float radius;
    private float mass;
    private float semiMinorAxis;
    private float semiMajorAxis;
    private float eccentricity;
    private float[] foci1;
    private float[] foci2;
    private float[] centre;
    private float[] localCentreVector;
    private float currentTheta;
    private float cosineEllipseRotation;
    private float sineEllipseRotation;
    private float distanceFromFoci;
    private List<OrbitalDetails> orbitingBodies;

    public OrbitalDetails(float radius,
        float mass,
        List<OrbitalDetails> orbitingBodies) {

        this.radius = radius;
        this.mass = mass;
        this.orbitingBodies = orbitingBodies;
    }

    public OrbitalDetails(float radius,
        float mass,
        float semiMinorAxis,
        float semiMajorAxis,
        float eccentricity,
        Vector3 foci1,
        Vector3 foci2,
        Vector3 centre,
        Vector3 localCentreVector,
        float currentTheta,
        float cosineEllipseRotation,
        float sineEllipseRotation,
        float distanceFromFoci,
        List<OrbitalDetails> orbitingBodies) : this(radius, mass, orbitingBodies) {

        this.semiMinorAxis = semiMinorAxis;
        this.semiMajorAxis = semiMajorAxis;
        this.eccentricity = eccentricity;
        this.foci1 = new float[3] {foci1.x, foci1.y, foci1.z};
        this.foci2 = new float[3] {foci2.x, foci2.y, foci2.z};
        this.centre = new float[3] {centre.x, centre.y, centre.z};
        this.localCentreVector = new float[3] {localCentreVector.x, localCentreVector.y, localCentreVector.z};
        this.currentTheta = currentTheta;
        this.cosineEllipseRotation = cosineEllipseRotation;
        this.sineEllipseRotation = sineEllipseRotation;
        this.distanceFromFoci = distanceFromFoci;
    }

    public float getRadius() {
        return radius;
    }

    public float getMass() {
        return mass;
    }

    public float getSemiMinorAxis() {
        return semiMinorAxis;
    }

    public float getSemiMajorAxis() {
        return semiMajorAxis;
    }

    public float getEccentricity() {
        return eccentricity;
    }

    public float[] getFoci1() {
        return foci1;
    }

    public float[] getFoci2() {
        return foci2;
    }

    public float[] getCentre() {
        return centre;
    }

    public float[] getLocalCentreVector() {
        return localCentreVector;
    }

    public float getCurrentTheta() {
        return currentTheta;
    }

    public float getCosineEllipseRotation() {
        return cosineEllipseRotation;
    }

    public float getSineEllipseRotation() {
        return sineEllipseRotation;
    }

    public float getDistanceFromFoci() {
        return distanceFromFoci;
    }
    
    public List<OrbitalDetails> getOrbitingBodies() {
        return orbitingBodies;
    }

    public void addOrbitingBody(OrbitalDetails orbitalDetails) {
        orbitingBodies.Add(orbitalDetails);
    }
}
