using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Liquid : MonoBehaviour
{
    private Vector3 position;
    private GameObject gameObject;
    private int sizeX;
    private int sizeY;
    public struct Bound
    {
        public float top;
        public float right;
        public float bottom;
        public float left;
    }

    public Bound bound;

    public Liquid(Vector3 position, int sizeX, int sizeY, GameObject gameObject, Bound bound)
    {
        this.position = position;
        this.gameObject = gameObject;
        this.sizeY = sizeY;
        this.sizeX = sizeX;
        this.bound = bound;
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

    public Bound getBound()
    {
        return this.bound;
    }
}
