using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Explosives : Weapon
{
    public GameObject safetyPin;
    protected bool exploded;
    public float explodeAfterSeconds;
    public AudioSource audioSource;
    public float explosionForce;
    public float explosionRadius;
    public GameObject explosionFX;

    public abstract void Explode();
    public abstract void RemoveSafetyPin();
}
