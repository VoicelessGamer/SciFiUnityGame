using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterBox : MonoBehaviour
{
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Player.transform.position;
        this.GetComponent<BoxCollider2D>().transform.position = Player.GetComponent<BoxCollider2D>().transform.position;
        this.GetComponent<Rigidbody2D>().transform.position = Player.GetComponent<Rigidbody2D>().transform.position;
        this.GetComponent<Rigidbody2D>().position = Player.GetComponent<Rigidbody2D>().position;
        this.GetComponent<Rigidbody2D>().velocity = Player.GetComponent<PlayerController>().velocity;
    }
}