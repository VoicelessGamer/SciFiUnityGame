using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    public bool controlEnabled = true;

    public float movementSpeed;

    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(controlEnabled) {
            //get input
            float rotation = -(Input.GetAxis("Horizontal") * rotationSpeed);
            float translation = Input.GetAxis("Vertical") * movementSpeed;

            //Multiply by delta time to do transformations over seconds rather than frames
            rotation *= Time.deltaTime;
            translation *= Time.deltaTime;

            //update
            transform.Translate(0, translation, 0);
            transform.Rotate(0, 0, rotation);
        }
    }
}
