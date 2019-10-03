using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {

    public GameObject orbitingObject;
    public float minimumPlanetSeparation;
    
    public float fociRange;

    private float semiMinorAxis;
    private float semiMajorAxis;
    private float eccentricity;
    private Vector3 foci1;
    private Vector3 foci2;
    private Vector3 centre;

    // Start is called before the first frame update
    void Start()
    {
        float minimumFociDistance = ((CircleCollider2D)orbitingObject.GetComponent(typeof(CircleCollider2D))).radius + 
            ((CircleCollider2D)gameObject.GetComponent(typeof(CircleCollider2D))).radius + minimumPlanetSeparation;

        foci1 = orbitingObject.transform.position;
        foci2 = new Vector3(Random.Range(minimumFociDistance, minimumFociDistance + fociRange), 0, 0);

        centre = new Vector3((foci2.x - foci1.x) / 2, (foci2.y - foci1.y) / 2, 0);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
