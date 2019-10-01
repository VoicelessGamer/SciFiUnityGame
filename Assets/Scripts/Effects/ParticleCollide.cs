using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollide : MonoBehaviour
{
    public Transform particle;

    private ParticleSystem.EmissionModule emissionModule;

    // Start is called before the first frame update
    void Start()
    {
        emissionModule = particle.GetComponent<ParticleSystem>().emission;
        emissionModule.enabled = false;
    }

    // Update is called once per frame
    void OnTriggerEnter2D()
    {
        emissionModule.enabled = true;
        particle.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1);

        StartCoroutine(stopParticles());
    }

    IEnumerator stopParticles()
    {
        yield return new WaitForSeconds(.4f);

        emissionModule.enabled = false;
    }
}
