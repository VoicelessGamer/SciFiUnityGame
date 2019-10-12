using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingBody : CelestialBody {
    
    [Header("Orbit Details")]
    public float minFociRadius;
    public float maxFociRadius;
    //minBodySeparation - This value plus the sum of the radii of this body and the orbiting body is the minimum 
    //distance between the two bodies when at periapsis
    public float minBodySeparation;
    //minBodySeparation plus the fociBodySeparationRange is the maximum separation between either foci and this body
    public float fociBodySeparationRange;
    
    private float semiMinorAxis;
    private float semiMajorAxis;
    private float eccentricity;
    private Vector3 foci1;
    private Vector3 foci2;
    private Vector3 centre;
    private Vector3 localCentreVector;
    private float currentTheta;
    private float cosineEllipseRotation;
    private float sineEllipseRotation;
    private float distanceFromFoci;

    public void loadDetails(OrbitalDetails orbitalDetails) {
        base.loadDetails(orbitalDetails.getRadius(), orbitalDetails.getMass());

        semiMinorAxis = orbitalDetails.getSemiMinorAxis();
        semiMajorAxis = orbitalDetails.getSemiMajorAxis();
        eccentricity = orbitalDetails.getEccentricity();
        foci1 = new Vector3(orbitalDetails.getFoci1()[0], orbitalDetails.getFoci1()[1], orbitalDetails.getFoci1()[2]);
        foci2 = new Vector3(orbitalDetails.getFoci2()[0], orbitalDetails.getFoci2()[1], orbitalDetails.getFoci2()[2]);
        centre = new Vector3(orbitalDetails.getCentre()[0], orbitalDetails.getCentre()[1], orbitalDetails.getCentre()[2]);
        localCentreVector = new Vector3(orbitalDetails.getLocalCentreVector()[0], orbitalDetails.getLocalCentreVector()[1], orbitalDetails.getLocalCentreVector()[2]);
        currentTheta = orbitalDetails.getCurrentTheta();
        cosineEllipseRotation = orbitalDetails.getCosineEllipseRotation();
        sineEllipseRotation = orbitalDetails.getSineEllipseRotation();
        distanceFromFoci = orbitalDetails.getDistanceFromFoci();

        gameObject.transform.localPosition = getPositionInOrbit(currentTheta);
    }

    public void setupOrbit() {

        float fociRadius = Random.Range(minFociRadius, maxFociRadius);
        float fociRotation = Random.Range(0, 360);

        //first foci point is the centre of the orbiting object
        //second foci point is randomly chosen distance and rotation from the first
        foci1 = gameObject.transform.parent.position;
        foci2 = new Vector3(foci1.x + (fociRadius * Mathf.Sin(fociRotation)), foci1.y + (fociRadius * Mathf.Cos(fociRotation)), 0);

        //radius of both planetary object added together with the minimum separation between the 2 planetary objects
        float minDistanceFromFoci = ((CircleCollider2D)gameObject.transform.parent.GetComponent(typeof(CircleCollider2D))).radius +
            ((CircleCollider2D)gameObject.GetComponent(typeof(CircleCollider2D))).radius + minBodySeparation;

        //minimum distance planet centre should be from either foci to ensure no collision with orbiting object
        distanceFromFoci = Random.Range(minDistanceFromFoci, minDistanceFromFoci + fociBodySeparationRange);

        //distance between the 2 foci points
        float fociDistance = Mathf.Sqrt(Mathf.Pow(foci2.x - foci1.x, 2.0f) + Mathf.Pow(foci2.y - foci1.y, 2.0f));

        //centre point of the ellipse
        centre = new Vector3(foci1.x + ((foci2.x - foci1.x) / 2), foci1.y + ((foci2.y - foci1.y) / 2), 0);

        //Position vector of the foci centre with respect to the body being orbited
        localCentreVector = new Vector3(foci1.x - centre.x, foci1.y - centre.y, 0);

        semiMajorAxis = (fociDistance / 2) + distanceFromFoci;

        eccentricity = (fociDistance / 2) / semiMajorAxis;

        semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - Mathf.Pow(eccentricity, 2.0f));

        float ellipseRotation = Mathf.Atan((foci2.y - foci1.y) / (foci2.x - foci1.x));
        cosineEllipseRotation = Mathf.Cos(ellipseRotation);
        sineEllipseRotation = Mathf.Sin(ellipseRotation);

        currentTheta = Random.Range(0, 360);

        gameObject.transform.localPosition = getPositionInOrbit(currentTheta);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTheta += (calculateOrbitalVelocity() * Time.deltaTime);
        gameObject.transform.localPosition = getPositionInOrbit(currentTheta);
    }

    private Vector3 getPositionInOrbit(float theta) {
        theta = theta * Mathf.Deg2Rad;
        
        return new Vector3((semiMajorAxis * Mathf.Cos(theta) * cosineEllipseRotation) - (semiMinorAxis * Mathf.Sin(theta) * sineEllipseRotation) - localCentreVector.x,
            (semiMajorAxis * Mathf.Cos(theta) * sineEllipseRotation) + (semiMinorAxis * Mathf.Sin(theta) * cosineEllipseRotation) - localCentreVector.y,
            0);
    }

    public float calculateOrbitalVelocity() {
        return (Mathf.Sqrt((gravitationalConstant * ((CelestialBody)gameObject.transform.parent.GetComponent(typeof(CelestialBody))).mass) / Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - foci1.x, 2.0f) + Mathf.Pow(gameObject.transform.position.y - foci1.y, 2.0f))));
    }    

    public float getSemiMinorAxis() {
        return semiMinorAxis;
    }

    public void setSemiMinorAxis(float semiMinorAxis) {
        this.semiMinorAxis = semiMinorAxis;
    }

    public float getSemiMajorAxis() {
        return semiMajorAxis;
    }

    public void setSemiMajorAxis(float semiMajorAxis) {
        this.semiMajorAxis = semiMajorAxis;
    }

    public float getEccentricity() {
        return eccentricity;
    }

    public void setEccentricity(float eccentricity) {
        this.eccentricity = eccentricity;
    }

    public Vector3 getFoci1() {
        return foci1;
    }

    public void setFoci1(Vector3 foci1) {
        this.foci1 = foci1;
    }

    public Vector3 getFoci2() {
        return foci2;
    }

    public void setFoci2(Vector3 foci2) {
        this.foci2 = foci2;
    }

    public Vector3 getCentre() {
        return centre;
    }

    public void setCentre(Vector3 centre) {
        this.centre = centre;
    }

    public Vector3 getLocalCentreVector() {
        return localCentreVector;
    }

    public void setLocalCentreVector(Vector3 localCentreVector) {
        this.localCentreVector = localCentreVector;
    }

    public float getCurrentTheta() {
        return currentTheta;
    }

    public void setCurrentTheta(float currentTheta) {
        this.currentTheta = currentTheta;
    }

    public float getCosineEllipseRotation() {
        return cosineEllipseRotation;
    }

    public void setCosineEllipseRotation(float cosineEllipseRotation) {
        this.cosineEllipseRotation = cosineEllipseRotation;
    }

    public float getSineEllipseRotation() {
        return sineEllipseRotation;
    }

    public void setSineEllipseRotation(float sineEllipseRotation) {
        this.sineEllipseRotation = sineEllipseRotation;
    }

    public float getDistanceFromFoci() {
        return distanceFromFoci;
    }

    public void setDistanceFromFoci(float distanceFromFoci) {
        this.distanceFromFoci = distanceFromFoci;
    }
}
