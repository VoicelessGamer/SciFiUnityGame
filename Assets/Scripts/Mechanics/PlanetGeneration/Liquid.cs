using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    private Vector3 position;
    private GameObject gameObject;

    public Liquid(Vector3 position, int sizeX, int sizeY, GameObject gameObject)
    {
        this.position = position;
        this.gameObject = gameObject;
    }

    public Vector3 getPosition()
    {
        return this.position;
    }

    public GameObject getGameObject()
    {
        return this.gameObject;
    }
}
