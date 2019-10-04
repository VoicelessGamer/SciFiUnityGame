using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {

    public GameObject orbitingObject;
    public float minimumFociRadius;
    public float maximumFociRadius;
    public float minimumFociSeparation;
    public float fociSeparationRange;
    public float planetVelocity;
    
    private float semiMinorAxis;
    private float semiMajorAxis;
    private float eccentricity;
    private Vector3 foci1;
    private Vector3 foci2;
    private Vector3 centre;

    //TESTING
    public GameObject testObject;
    public GameObject testObject2;
    public GameObject testObject3;

    private float currentTheta;

    // Start is called before the first frame update
    void Start()
    {

        float fociRadius = Random.Range(minimumFociRadius, maximumFociRadius);
        float fociRotation = Random.Range(0, 360);

        //first foci point is the centre of the orbiting object
        //second foci point is randomly chosen
        foci1 = orbitingObject.transform.position;
        foci2 = new Vector3(fociRadius * Mathf.Sin(fociRotation), fociRadius * Mathf.Cos(fociRotation), 0);

        float centreMassDistance = ((CircleCollider2D)orbitingObject.GetComponent(typeof(CircleCollider2D))).radius +
            ((CircleCollider2D)gameObject.GetComponent(typeof(CircleCollider2D))).radius;

        //randomly generate the planet position
        //gameObject.transform.position.x = r1 + r2 + Random.Range(minimumPlanetDistance, maximumPlanetDistance);

        //minimum distance planet centre should be from either foci to ensure no collision with orbiting object
        float minDistanceFromFoci = centreMassDistance + minimumFociSeparation;
        float maxDistanceFromFoci = minDistanceFromFoci + fociSeparationRange;
        float distanceFromFoci = Random.Range(minDistanceFromFoci, maxDistanceFromFoci);

        //distance between the 2 foci points
        float fociDistance = Mathf.Sqrt(Mathf.Pow(foci2.x - foci1.x, 2.0f) + Mathf.Pow(foci2.y - foci1.y, 2.0f));

        //centre point of the ellipse
        centre = new Vector3((foci2.x - foci1.x) / 2, (foci2.y - foci1.y) / 2, 0);

        semiMajorAxis = (fociDistance / 2) + distanceFromFoci;
        //semiMajorAxis = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - centre.x, 2.0f) + Mathf.Pow(gameObject.transform.position.y - centre.y, 2.0f));

        //float centreToFoci = Mathf.Sqrt(Mathf.Pow(foci2.x - centre.x, 2.0f) + Mathf.Pow(foci2.y - centre.y, 2.0f));

        eccentricity = (fociDistance / 2) / semiMajorAxis;

        semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - Mathf.Pow(eccentricity, 2.0f));

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
    void Update()
    {
        currentTheta += (planetVelocity * Time.deltaTime);
        gameObject.transform.position = getPositionInOrbit(currentTheta);

    }

    private Vector3 getPositionInOrbit(float theta) {
        return new Vector3((centre.x - foci1.x) + (semiMajorAxis * Mathf.Cos(theta)), (centre.y - foci1.y) + (semiMinorAxis * Mathf.Sin(theta)), 0);
    }
}
