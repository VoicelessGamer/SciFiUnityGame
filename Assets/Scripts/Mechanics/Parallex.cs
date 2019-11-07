using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallex : MonoBehaviour
{
    private float length, startpos, height, startposY;
    public GameObject cam;
    public float parallaxEffect;
    public float parallaxEffectY;
    // Start is called before the first frame update
    void Start()
    {
        startpos = cam.transform.position.x;
        startposY = cam.transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        float tempY = (cam.transform.position.y * (1 - parallaxEffectY));
        float distY = (cam.transform.position.y * parallaxEffectY);

        transform.position = new Vector3(startpos + dist, startposY + distY, transform.position.z);

        if (temp > startpos + length)
            startpos += length;
        else if (temp < startpos - length)
            startpos -= length;

        if (tempY > startposY + height)
            startposY += height;
        else if (tempY < startposY - height)
            startposY -= height;
    }
}
