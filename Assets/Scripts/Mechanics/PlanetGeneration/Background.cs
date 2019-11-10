using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Vector3 position;
    private GameObject gameObject;
    private int orderLayer;
    private float parallexX;
    private float parallexY;
    private Sprite sprite;


    public Background(Vector3 position, int orderLayer, float parallexX, float parallexY, GameObject gameObject, Sprite sprite)
    {
        this.position = position;
        this.gameObject = gameObject;
        this.orderLayer = orderLayer;
        this.parallexX = parallexX;
        this.parallexY = parallexY;
        this.sprite = sprite;
    }

    public Vector3 getPosition()
    {
        return this.position;
    }

    public GameObject getGameObject()
    {
        return this.gameObject;
    }

    public int getOrderLayer()
    {
        return this.orderLayer;
    }

    public float getParallexX()
    {
        return this.parallexX;
    }

    public float getParallexY()
    {
        return this.parallexY;
    }

    public Sprite getSprite()
    {
        return this.sprite;
    }

    
}
