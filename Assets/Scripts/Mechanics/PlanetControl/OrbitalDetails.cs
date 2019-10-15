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
        return this.radius;
    }

    public void setRadius(float radius) {
        this.radius = radius;
    }
    public float getMass() {
        return this.mass;
    }

    public void setMass(float mass) {
        this.mass = mass;
    }

    public float getSemiMinorAxis() {
        return this.semiMinorAxis;
    }

    public void setSemiMinorAxis(float semiMinorAxis) {
        this.semiMinorAxis = semiMinorAxis;
    }

    public float getSemiMajorAxis() {
        return this.semiMajorAxis;
    }

    public void setSemiMajorAxis(float semiMajorAxis) {
        this.semiMajorAxis = semiMajorAxis;
    }

    public float getEccentricity() {
        return this.eccentricity;
    }

    public void setEccentricity(float eccentricity) {
        this.eccentricity = eccentricity;
    }

    public float[] getFoci1() {
        return this.foci1;
    }

    public void setFoci1(float[] foci1) {
        this.foci1 = foci1;
    }

    public float[] getFoci2() {
        return this.foci2;
    }

    public void setFoci2(float[] foci2) {
        this.foci2 = foci2;
    }

    public float[] getCentre() {
        return this.centre;
    }

    public void setCentre(float[] centre) {
        this.centre = centre;
    }

    public float[] getLocalCentreVector() {
        return this.localCentreVector;
    }

    public void setLocalCentreVector(float[] localCentreVector) {
        this.localCentreVector = localCentreVector;
    }

    public float getCurrentTheta() {
        return this.currentTheta;
    }

    public void setCurrentTheta(float currentTheta) {
        this.currentTheta = currentTheta;
    }

    public float getCosineEllipseRotation() {
        return this.cosineEllipseRotation;
    }

    public void setCosineEllipseRotation(float cosineEllipseRotation) {
        this.cosineEllipseRotation = cosineEllipseRotation;
    }

    public float getSineEllipseRotation() {
        return this.sineEllipseRotation;
    }

    public void setSineEllipseRotation(float sineEllipseRotation) {
        this.sineEllipseRotation = sineEllipseRotation;
    }

    public float getDistanceFromFoci() {
        return this.distanceFromFoci;
    }

    public void setDistanceFromFoci(float distanceFromFoci) {
        this.distanceFromFoci = distanceFromFoci;
    }
    
    public List<OrbitalDetails> getOrbitingBodies() {
        return this.orbitingBodies;
    }

    public void setOrbitingBodies(List<OrbitalDetails> orbitingBodies) {
        this.orbitingBodies = orbitingBodies;
    }

    public void addOrbitingBody(OrbitalDetails orbitalDetails) {
        this.orbitingBodies.Add(orbitalDetails);
    }
}
