using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {

    public GameObject orbitingObject;
    public float minFociRadius;
    public float maxFociRadius;
    public float minPlanetSeparation;
    public float fociSeparationRange;
    public float planetVelocity;
    
    private float semiMinorAxis;
    private float semiMajorAxis;
    private float eccentricity;
    private Vector3 foci1;
    private Vector3 foci2;
    private Vector3 centre;
    private float ellipseRotation;

    //TESTING
    public GameObject testObject;
    public GameObject testObject2;
    public GameObject testObject3;
    private float currentTheta;

    // Start is called before the first frame update
    void Start() {

        float fociRadius = Random.Range(minFociRadius, maxFociRadius);
        float fociRotation = Random.Range(0, 360);

        //first foci point is the centre of the orbiting object
        //second foci point is randomly chosen distance and rotation from the first
        foci1 = orbitingObject.transform.position;
        foci2 = new Vector3(fociRadius * Mathf.Sin(fociRotation), fociRadius * Mathf.Cos(fociRotation), 0);

        //radius of both planetary object added together with the minimum separation between the 2 planetary objects
        float minDistanceFromFoci = ((CircleCollider2D)orbitingObject.GetComponent(typeof(CircleCollider2D))).radius +
            ((CircleCollider2D)gameObject.GetComponent(typeof(CircleCollider2D))).radius + minPlanetSeparation;

        //minimum distance planet centre should be from either foci to ensure no collision with orbiting object
        float distanceFromFoci = Random.Range(minDistanceFromFoci, minDistanceFromFoci + fociSeparationRange);

        //distance between the 2 foci points
        float fociDistance = Mathf.Sqrt(Mathf.Pow(foci2.x - foci1.x, 2.0f) + Mathf.Pow(foci2.y - foci1.y, 2.0f));

        //centre point of the ellipse
        centre = new Vector3((foci2.x - foci1.x) / 2, (foci2.y - foci1.y) / 2, 0);

        semiMajorAxis = (fociDistance / 2) + distanceFromFoci;

        eccentricity = (fociDistance / 2) / semiMajorAxis;

        semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - Mathf.Pow(eccentricity, 2.0f));

        ellipseRotation = Mathf.Atan((foci2.y - foci1.y) / (foci2.x - foci1.x));

        //Testing        
        GameObject go = (GameObject)Instantiate(testObject2, foci1, Quaternion.identity);
        go = (GameObject)Instantiate(testObject2, foci2, Quaternion.identity);
        go = (GameObject)Instantiate(testObject3, centre, Quaternion.identity);

        for(int i = 0; i < 360; i++) {
            go = (GameObject)Instantiate(testObject, getPositionInOrbit(i), Quaternion.identity);
        }

        currentTheta = 0;

        gameObject.transform.position = getPositionInOrbit(currentTheta);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTheta += (planetVelocity * Time.deltaTime);
        gameObject.transform.position = getPositionInOrbit(currentTheta);

    }

    private Vector3 getPositionInOrbit(float theta) {
        theta = theta * Mathf.Deg2Rad;
        
        return new Vector3((semiMajorAxis * Mathf.Cos(theta) * Mathf.Cos(ellipseRotation)) - (semiMinorAxis * Mathf.Sin(theta) * Mathf.Sin(ellipseRotation)) + centre.x,
            (semiMajorAxis * Mathf.Cos(theta) * Mathf.Sin(ellipseRotation)) + (semiMinorAxis * Mathf.Sin(theta) * Mathf.Cos(ellipseRotation)) + centre.y,
            0);
    }
}
