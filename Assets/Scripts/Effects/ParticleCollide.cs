using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollide : MonoBehaviour
{
    public Transform particle;

    // Start is called before the first frame update
    void Start()
    {
        particle.GetComponent<ParticleSystem>().enableEmission = false;
    }

    // Update is called once per frame
    void OnTriggerEnter2D()
    {
        particle.GetComponent<ParticleSystem>().enableEmission = true;
        particle.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1);

        StartCoroutine(stopParticles());
    }

    IEnumerator stopParticles()
    {
        yield return new WaitForSeconds(.4f);

        particle.GetComponent<ParticleSystem>().enableEmission = false;
    }
}
