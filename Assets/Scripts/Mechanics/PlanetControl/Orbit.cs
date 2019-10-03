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
    private Transform foci1;
    private Transform foci2;
    private Transform centre;

    // Start is called before the first frame update
    void Start()
    {
        float minimumFociDistance = ((CircleCollider2D)orbitingObject.GetComponent(typeof(CircleCollider2D))).radius + 
            ((CircleCollider2D)gameObject.GetComponent(typeof(CircleCollider2D))).radius + minimumPlanetSeparation;

        foci1 = orbitingObject.transform;
        foci2 = Random.Range(minimumFociDistance, minimumFociDistance + fociRange);

        centre = new Vector3((foci2.position.x - foci1.position.x) / 2, (foci2.position.y - foci1.position.y) / 2, 0);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
