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
        this.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
    }
}
