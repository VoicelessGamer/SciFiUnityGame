using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    private Vector3 position;
    private GameObject gameObject;
    private int sizeX;
    private int sizeY;

    public Liquid(Vector3 position, int sizeX, int sizeY, GameObject gameObject)
    {
        this.position = position;
        this.gameObject = gameObject;
        this.sizeY = sizeY;
        this.sizeX = sizeX;
    }

    public Vector3 getPosition()
    {
        return this.position;
    }

    public GameObject getGameObject()
    {
        return this.gameObject;
    }

    public int getSizeY()
    {
        return this.sizeY;
    }

    public int getSizeX()
    {
        return this.sizeX;
    }
}
