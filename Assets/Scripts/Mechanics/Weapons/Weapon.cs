using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    public int damage;

    protected float cooldown;

    protected abstract void attack();
}
